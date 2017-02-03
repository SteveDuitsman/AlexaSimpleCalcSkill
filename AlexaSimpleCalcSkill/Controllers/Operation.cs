using System.Collections.Generic;

namespace AlexaSimpleCalcSkill.Controllers
{
  public class OperationFactory
  {
    public OperationEnum Create(string op)
    {
      OperationEnum result = OperationEnum.None;
      if (Add.Contains(op.ToLowerInvariant()))
      {
        result = OperationEnum.Add;
      }
      else if (Subtract.Contains(op.ToLowerInvariant()))
      {
        result = OperationEnum.Subtract;
      }
      else if (Multiply.Contains(op.ToLowerInvariant()))
      {
        result = OperationEnum.Multiply;
      }
      else if (Divide.Contains(op.ToLowerInvariant()))
      {
        result = OperationEnum.Divide;
      }
      else if (SquareRoot.Contains(op.ToLowerInvariant()))
      {
        result = OperationEnum.SquareRoot;
      }
      else if (Squared.Contains(op.ToLowerInvariant()))
      {
        result = OperationEnum.Square;
      }
      else if (ToPower.Contains(op.ToLowerInvariant()))
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