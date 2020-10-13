using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Utilities
{
    public static class Operators
    {
        //все типы данных
        //string
        //double
        //int
        //bool
        public static object UniversalSum(object left, object right)
        {
            if (left is int && right is int)
                return (int)left + (int)right;
            else if (left is double && right is int)
                return (double)left + (int)right;
            else if (left is int && right is double)
                return (int)left + (double)right;
            else if (left is double && right is double)
                return (double)left + (double)right;
            else if (left is bool && right is bool)
                return (bool)left || (bool)right;
            else
                return left.ToString() + right.ToString();
        }

        public static object UniversalMinus(object left, object right)
        {
            if (left is int && right is int)
                return (int)left - (int)right;
            else if (left is double && right is int)
                return (double)left - (int)right;
            else if (left is int && right is double)
                return (int)left - (double)right;
            else if (left is double && right is double)
                return (double)left - (double)right;
            else if (left is bool && right is bool)
                return !((bool)left || (bool)right);
            throw new InvalidCastException();
        }

        public static object UniversalMult(object left, object right)
        {
            if (left is int && right is int)
                return (int)left * (int)right;
            else if (left is double && right is int)
                return (double)left * (int)right;
            else if (left is int && right is double)
                return (int)left * (double)right;
            else if (left is double && right is double)
                return (double)left * (double)right;
            else if (left is bool && right is bool)
                return (bool)left ^ (bool)right;
            throw new InvalidCastException();
        }
        
        public static object UniversalPow(object left, object right)
        {
            if (left is int && right is int)
                return Math.Pow((int)left, (int)right);
            else if (left is double && right is int)
                return Math.Pow((double)left, (int)right);
            else if (left is int && right is double)
                return Math.Pow((int)left, (double)right);
            else if (left is double && right is double)
                return Math.Pow((double)left, (double)right);
            throw new InvalidCastException();
        }

        public static object UniversalDiv(object left, object right)
        {
            if (left is int && right is int)
                return (int)left / (int)right;
            else if (left is double && right is int)
                return (double)left / (int)right;
            else if (left is int && right is double)
                return (int)left / (double)right;
            else if (left is double && right is double)
                return (double)left / (double)right;
            else if (left is bool && right is bool)
                return !((bool)left ^ (bool)right);
            throw new InvalidCastException();
        }
        
        public static object UniversalRemainder(object left, object right)
        {
            if (left is int && right is int)
                return (int)left % (int)right;
            else if (left is double && right is int)
                return (double)left % (int)right;
            else if (left is int && right is double)
                return (int)left % (double)right;
            else if (left is double && right is double)
                return (double)left % (double)right;
            throw new InvalidCastException();
        }

        public static bool UniversalMore(object left, object right)
        {
            if (left is int && right is int)
                return (int)left > (int)right;
            else if (left is double && right is int)
                return (double)left > (int)right;
            else if (left is int && right is double)
                return (int)left > (double)right;
            else if (left is double && right is double)
                return (double)left > (double)right;
            else if (left is bool && right is bool)
                return (bool)left && !(bool)right;
            return false;
        }

        public static bool UniversalLess(object left, object right)
        {
            if (left is int && right is int)
                return (int)left < (int)right;
            else if (left is double && right is int)
                return (double)left < (int)right;
            else if (left is int && right is double)
                return (int)left < (double)right;
            else if (left is double && right is double)
                return (double)left < (double)right;
            else if (left is bool && right is bool)
                return !(bool)left && (bool)right;
            return false;
        }

        public static bool UniversalEqual(object left, object right)
        {
            if (left is int && right is int)
                return (int)left == (int)right;
            else if (left is double && right is int)
                return (double)left == (int)right;
            else if (left is int && right is double)
                return (int)left == (double)right;
            else if (left is double && right is double)
                return (double)left == (double)right;
            else if (left is bool && right is bool)
                return (bool)left == (bool)right;
            else if (left is string || right is string)
                return left.ToString() == right.ToString();
            return false;
        }

        public static bool UniversalUnEqual(object left, object right)
        {
            return !UniversalEqual(left, right);
        }
    }
}
