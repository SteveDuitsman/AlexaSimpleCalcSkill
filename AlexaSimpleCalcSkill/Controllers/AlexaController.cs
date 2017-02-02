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

      var response = new AlexaResponse("Hi. I'm SimpleCalculator. This is going to be a SimpleCalculator. Say it again? Yes or no?");
      response.Session.MemberId = request.MemberId;
      response.Response.Card.Title = "Simple Calculator";
      response.Response.Card.Content = "Calculator";
      response.Response.Reprompt.OutputSpeech.Text = "Say it again? Yes or no?";
      response.Response.ShouldEndSession = true;

      //if (request.Intent == "AMAZON.YesIntent")
      //{

      //}

      //if (requestIntent == "AMAZON.NoIntent")
      //{
      //  response.Response.OutputSpeech.Text = "Ok. I'll leave you be for now.";
      //  response.Response.ShouldEndSession = true;
      //}

      return response;

    }
  }
}