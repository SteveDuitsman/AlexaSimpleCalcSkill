using System.Web;
using System.Web.Mvc;

namespace AlexaSimpleCalcSkill
{
  public class FilterConfig
  {
    public static void RegisterGlobalFilters(GlobalFilterCollection filters)
    {
      filters.Add(new HandleErrorAttribute());
    }
  }
}
