﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlexaSimpleCalcSkill.Data
{
  public class Requests
  {
    public Request Create(Request request)
    {
      using (var db = new SimpleCalcSkillEntities())
      {
        try
        {
          var member = db.Members.FirstOrDefault(m => m.AlexaUserId == request.UserId);

          if (member == null)
          {
            request.Member = new Member()
            {
              AlexaUserId = request.UserId,
              CreatedDate = DateTime.UtcNow,
              LastRequestDate = DateTime.UtcNow,
              RequestCount = 1
            };

            db.Requests.Add(request);
          }
          else
          {
            member.Requests.Add(request);
          }

          db.SaveChanges();
        }
        catch (Exception e)
        {
          var text = e.Message;
          throw;
        }
      }
      return request;
    }
  }
}