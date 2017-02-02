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
                                            //SlotsList = alexaRequest.Request.Intent.GetSlots(),
                                            DateCreated = DateTime.UtcNow
                                          });
      AlexaResponse response = null;
      switch (request.Type)
      {
        case "LaunchRequest":
          response = LaunchRequestHandler(request);
          break;
        case "IntentRequest":
          response = IntentRequestHandler(request);
          break;
        case "SessionEndedRequest":
          response = SessionEndedRequestHandler(request);
          break;
      }
      return response;
    }

    private AlexaResponse IntentRequestHandler(Request request)
    {
      AlexaResponse response = null;
      switch (request.Intent)
      {
        case "CalculationIntent":
          response = CalculationIntentHandler(request);
          break;
        case "ChainedCalculationIntent":
          response = ChainedCalculationIntentHandler(request);
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

    private AlexaResponse ChainedCalculationIntentHandler(Request request)
    {
      var response = new AlexaResponse("I dont know how to chain calculations yet. Sorry.")
                     {
                       Response = {ShouldEndSession = true}
                     };
      return response;
    }

    private AlexaResponse CalculationIntentHandler(Request request)
    {
      var response = new AlexaResponse("I dont know how to do calculations yet. Sorry.")
      {
        Response = { ShouldEndSession = true }
      };
      return response;
    }

    private AlexaResponse CancelIntentHandler(Request request)
    {
      var response = new AlexaResponse("Canceled.")
      {
        Response = { ShouldEndSession = true }
      };
      return response;
    }

    private AlexaResponse StopIntentHandler(Request request)
    {
      var response = new AlexaResponse("Ok. I'll stop.")
      {
        Response = { ShouldEndSession = true }
      };
      return response;
    }


    private AlexaResponse LaunchRequestHandler(Request request)
    {
      var response = new AlexaResponse("Welcome to SimpleCalculator. I can do simple math for you. What would you like to know?");
      response.Session.MemberId = request.MemberId;
      response.Response.Card.Title = "Simple Calculator";
      response.Response.Card.Content = "Calculator";
      response.Response.Reprompt.OutputSpeech.Text = "Can I help with some math?";
      response.Response.ShouldEndSession = true;

      if (request.Intent == "AMAZON.NoIntent")
      {
        response.Response.OutputSpeech.Text = "Ok. I'll leave you be for now.";
        response.Response.ShouldEndSession = true;
      }

      return response;
    }

    private AlexaResponse SessionEndedRequestHandler(Request request)
    {
      return null;
    }
  }
}