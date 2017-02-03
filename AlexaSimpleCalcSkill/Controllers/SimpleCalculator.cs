using System;

namespace AlexaSimpleCalcSkill.Controllers
{
  public class SimpleCalculator
  {
    public double Calculate(double firstValue, OperationEnum operation, double secondValue)
    {
      var result = double.NaN;
      switch (operation)
      {
        case OperationEnum.Add:
          result = firstValue + secondValue;
          break;
        case OperationEnum.Subtract:
          result = firstValue - secondValue;
          break;
        case OperationEnum.Multiply:
          result = firstValue * secondValue;
          break;
        case OperationEnum.Divide:
          result = firstValue / secondValue;
          break;

        case OperationEnum.Square:
          result = firstValue * firstValue;
          break;
        case OperationEnum.SquareRoot:
          result = Math.Sqrt(firstValue);
          break;
        case OperationEnum.Power:
          result = Math.Pow(firstValue, secondValue);
          break;
      }
      return result;
    }
  }
}