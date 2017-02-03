using System.Collections.Generic;

namespace AlexaSimpleCalcSkill.Controllers
{
  public class OperationFactory
  {
    public OperationEnum Create(string op)
    {
      var result = OperationEnum.None;
      var lowerCaseOp = op.ToLowerInvariant();
      if (Add.Contains(lowerCaseOp))
      {
        result = OperationEnum.Add;
      }
      else if (Subtract.Contains(lowerCaseOp))
      {
        result = OperationEnum.Subtract;
      }
      else if (Multiply.Contains(lowerCaseOp))
      {
        result = OperationEnum.Multiply;
      }
      else if (Divide.Contains(lowerCaseOp))
      {
        result = OperationEnum.Divide;
      }
      else if (SquareRoot.Contains(lowerCaseOp))
      {
        result = OperationEnum.SquareRoot;
      }
      else if (Squared.Contains(lowerCaseOp))
      {
        result = OperationEnum.Square;
      }
      else if (ToPower.Contains(lowerCaseOp))
      {
        result = OperationEnum.Power;
      }
      return result;
    }

    private List<string> Add = new List<string> {"add", "plus"};
    private List<string> Subtract = new List<string> {"subtract", "minus"};
    private List<string> Multiply = new List<string> {"multiply", "times"};
    private List<string> Divide = new List<string> {"divide", "divided by"};
    private List<string> SquareRoot = new List<string> {"square root"};
    private List<string> Squared = new List<string> {"squared"};
    private List<string> ToPower = new List<string> {"to the power of"};
  }

  public enum OperationEnum
  {
    None,
    Add,
    Subtract,
    Multiply,
    Divide,
    SquareRoot,
    Square,
    Power
  }
}