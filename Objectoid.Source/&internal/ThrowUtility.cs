using System;
using System.Collections.Generic;
using System.Text;

namespace Objectoid.Source
{
    internal static class ThrowUtility
    {
        /// <summary>Tests the action to see if it throws an exception</summary>
        /// <param name="testAction">Test action</param>
        /// <param name="e">Thrown exception</param>
        /// <returns>True if the action completed without throwing an exception; otherwise false</returns>
        /// <exception cref="ArgumentNullException"><paramref name="testAction"/> is null</exception>
        public static bool Test(Action testAction, out Exception e)
        {
            try
            {
                testAction();
                e = null;
                return true;
            }
            catch when (testAction is null)
            {
                throw new ArgumentNullException(nameof(testAction));
            }
            catch (Exception ex)
            {
                e = ex;
                return false;
            }
        }
    }
}
