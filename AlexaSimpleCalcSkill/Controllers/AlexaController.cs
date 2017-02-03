using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AlexaSimpleCalcSkill.Data;
using AlexaSimpleCalcSkill.Models;

namespace AlexaSimpleCalcSkill.Controllers
{
  public class AlexaController : ApiController
  {
    [HttpPost, Route("api/alexa/simplecalculator")]
    public dynamic SimpleCalculator(AlexaRequest alexaRequest)
    {
      var request = new Requests().Create(new Data.Request
                                          {
                                            MemberId = (alexaRequest.Session.Attributes == null) ? 0 : alexaRequest.Session.Attributes.MemberId,
                                            Timestamp = alexaRequest.Request.Timestamp,
                                            Intent = (alexaRequest.Request.Intent == null) ? "" : alexaRequest.Request.Intent.Name,
                                            AppId = alexaRequest.Session.Application.ApplicationId,
                                            RequestId = alexaRequest.Request.RequestId,
                                            SessionId = alexaRequest.Session.SessionId,
                                            UserId = alexaRequest.Session.User.UserId,
                                            IsNew = alexaRequest.Session.New,
                                            Version = alexaRequest.Version,
                                            Type = alexaRequest.Request.Type,
                                            Reason = alexaRequest.Request.Reason,
                                            SlotsList = alexaRequest?.Request?.Intent?.GetSlots(),
                                            DateCreated = DateTime.UtcNow
                                          });
      AlexaResponse response = null;
      switch (request.Type)
      {
        case "LaunchRequest":
          response = LaunchRequestHandler(request);
          break;
        case "IntentRequest":
          response = IntentRequestHandler(request, alexaRequest);
          break;
        case "SessionEndedRequest":
          response = SessionEndedRequestHandler(request);
          break;
      }
      return response;
    }

    private AlexaResponse IntentRequestHandler(Request request, AlexaRequest alexaRequest)
    {
      AlexaResponse response = null;
      switch (request.Intent)
      {
        case "CalculationIntent":
          response = CalculationIntentHandler(request);
          break;
        case "ChainedCalculationIntent":
          response = ChainedCalculationIntentHandler(request, alexaRequest);
          break;
        case "AMAZON.HelpIntent":
          response = HelpIntentHandler(request);
          break;
        case "AMAZON.StopIntent":
          response = StopIntentHandler(request);
          break;
        case "AMAZON.CancelIntent":
          response = CancelIntentHandler(request);
          break;
      }
      return response;
    }


    //
    // Intent Handlers
    //

    private AlexaResponse ChainedCalculationIntentHandler(Request request, AlexaRequest alexaRequest)
    {
      var response = new AlexaResponse("I don't understand how to chain that calculation. Can you try again?", false);

      try
      {
        var firstValue = request.SlotsList.FirstOrDefault(s => s.Key == FIRST_VALUE).Value;
        var op = request.SlotsList.FirstOrDefault(s => s.Key == OPERATOR).Value;

        if (HasPreviousAnswer(alexaRequest) && 
            firstValue != null && 
            op != null)
        {
          var actualOperation = new OperationFactory().Create(op);
          var actualFirstValue = GetDoubleFromSlotValue(firstValue);
          var previousAnswer = GetPreviousAnswer(alexaRequest);

          if (Math.Abs(actualFirstValue - 0.00) <= 0.000 && actualOperation == OperationEnum.Divide)
          {
            response = new AlexaResponse("Only Chuck Norris can do that.", false);
          }
          else
          {
            var answer = new SimpleCalculator().Calculate(previousAnswer, actualOperation, actualFirstValue);
            var message = $"{previousAnswer} {op} {firstValue} is {answer}";
            response = new AlexaResponse(message, false)
            {
              Session = { PreviousAnswer = answer }
            };
          }
        }
      }
      catch (Exception e)
      {
        response = new AlexaResponse("Something went wrong. It's my fault. Let's start over.", true);
      }
      return response;
    }

    private const string FIRST_VALUE = "FirstValue";
    private const string OPERATOR = "Operator";
    private const string SECOND_VALUE = "SecondValue";


    private AlexaResponse CalculationIntentHandler(Request request)
    {
      var response = new AlexaResponse("I don't understand what you're asking me to calculate. Can you try again?", false);

      try
      {
        var firstValue = request.SlotsList.FirstOrDefault(s => s.Key == FIRST_VALUE).Value;
        var op = request.SlotsList.FirstOrDefault(s => s.Key == OPERATOR).Value;
        var secondValue = request.SlotsList.FirstOrDefault(s => s.Key == SECOND_VALUE).Value;

        if (firstValue != null && op != null && secondValue != null)
        {
          var actualOperation = new OperationFactory().Create(op);
          var actualFirstValue = GetDoubleFromSlotValue(firstValue);
          var actualSecondValue = GetDoubleFromSlotValue(secondValue);

          if (Math.Abs(actualSecondValue - 0.00) <= 0.000 && actualOperation == OperationEnum.Divide)
          {
            response = new AlexaResponse("Only Chuck Norris can do that.", true);
          }
          else
          {
            var answer = new SimpleCalculator().Calculate(actualFirstValue, actualOperation, actualSecondValue);
            var message = $"{firstValue} {op} {secondValue} is {answer}";
            response = new AlexaResponse(message, false)
                       {
                         Session = {PreviousAnswer = answer}
                       };
          }
        }
      }
      catch (Exception e)
      {
        response = new AlexaResponse("Something went wrong. It's my fault. Let's start over.", true);
      }
      return response;
    }

    private AlexaResponse HelpIntentHandler(Request request)
    {
      var response = new AlexaResponse("To use SimpleCalculator, say things like, Alexa, ask SimpleCalculator what is 5 plus 5, or Alexa, what is 5 to the power of 5.", false);
      return response;
    }

    private AlexaResponse CancelIntentHandler(Request request)
    {
      var response = new AlexaResponse("Canceled.", true);
      return response;
    }

    private AlexaResponse StopIntentHandler(Request request)
    {
      var response = new AlexaResponse("Ok. I'll stop.", true)
                     {
                       Session = {PreviousAnswer = null}
                     };
      return response;
    }


    // 
    // Request Handlers
    // 

    private AlexaResponse LaunchRequestHandler(Request request)
    {
      var response = new AlexaResponse("Welcome to SimpleCalculator. I can do simple math for you. What would you like to know?");
      response.Session.MemberId = request.MemberId;
      response.Response.Card.Title = "Simple Calculator";
      response.Response.Card.Content = "Calculator";
      response.Response.Reprompt.OutputSpeech.Text = "Can I help with some math?";
      response.Response.ShouldEndSession = true;
      return response;
    }

    private AlexaResponse SessionEndedRequestHandler(Request request)
    {
      return null;
    }


    //
    // Helper Methods
    //
    private double GetDoubleFromSlotValue(string value)
    {
      var number = 0.0;
      if (!double.TryParse(value, out number))
      {
        throw new ArgumentOutOfRangeException("slot value should be a number");
      }
      return number;
    }

    private bool HasPreviousAnswer(AlexaRequest alexaRequest)
    {
      return alexaRequest.Session.Attributes.PreviousAnswer.HasValue;
    }

    private double GetPreviousAnswer(AlexaRequest alexaRequest)
    {
      return alexaRequest.Session.Attributes.PreviousAnswer ?? 0;
    }
  }
}