//------------------------------------------------------------------------------
// <copyright file="SecurityUtil.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

/*
 * SecurityUtil class
 *
 * Copyright (c) 1999 Microsoft Corporation
 */

namespace Viternus.Membership.Providers
{
  using System;
  using System.Collections;
  using System.Collections.Specialized;
  using System.Configuration.Provider;
  using System.Globalization;

  internal static class SecurityUtility
  {

    internal static string DefaultAppName
    {
      get
      {
        return "/";
      }
    }

    #region Internal Methods

    /// <summary>
    /// Returns the appName and commandTimeout values found in the NameValueCollection
    /// </summary>
    /// <param name="config"></param>
    /// <param name="appName"></param>
    /// <param name="commandTimeout"></param>
    internal static void InitializeCommonParameters(NameValueCollection config, out string appName, out int commandTimeout)
    {
      if (config == null)
        throw new ArgumentNullException("config");

      //Make a copy of the NameValueCollection so that we do not accidentally change any referenced values
      NameValueCollection values = new NameValueCollection(config);

      commandTimeout = SecurityUtility.GetIntValue(values, "commandTimeout", 30, true, 0);

      appName = values["applicationName"];
      if (string.IsNullOrEmpty(appName))
        appName = SecurityUtility.DefaultAppName;

      if (appName.Length > 256)
      {
        throw new ProviderException(StringResources.Provider_application_name_too_long);
      }

      values.Remove("applicationName");
      values.Remove("commandTimeout");
      if (values.Count > 0)
      {
        string attribUnrecognized = values.GetKey(0);
        if (!String.IsNullOrEmpty(attribUnrecognized))
          throw new ProviderException(String.Format(CultureInfo.CurrentCulture, StringResources.Provider_unrecognized_attribute, attribUnrecognized));
      }
    }

    /// <summary>
    /// Validates integrity of 'param' based on if it is Null, Empty, or is too big
    /// </summary>
    /// <remarks>We do not trim the parameters before checking with password parameters</remarks>
    /// <param name="param">The parameter to validate</param>
    /// <param name="maxSize">Specify '0' if maxSize should not be checked</param>
    /// <returns></returns>
    internal static bool ValidatePasswordParameter(ref string param, int maxSize)
    {
      return ValidateParameter(ref param, true, true, false, maxSize);
    }

    /// <summary>
    /// Validates integrity of 'param' based on if it is Null, Empty, contains commas, or is too big
    /// </summary>
    /// <param name="param">The parameter to validate</param>
    /// <param name="checkForNull">If true, validates that param is not null</param>
    /// <param name="checkIfEmpty">If true, validates that param is not blank</param>
    /// <param name="checkForCommas">If true, validates that param does not contain one or more commas</param>
    /// <param name="maxSize">Specify '0' if maxSize should not be checked</param>
    /// <returns></returns>
    internal static bool ValidateParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize)
    {
      if (null == param)
      {
        return !checkForNull;
      }

      param = param.Trim();
      return !((checkIfEmpty && param.Length < 1) || (maxSize > 0 && param.Length > maxSize) || (checkForCommas && param.Contains(",")));
    }

    /// <summary>
    /// Throws exception if 'param' is Null, Empty, or is too big
    /// </summary>
    /// <remarks>We do not trim the parameters before checking with password parameters</remarks>
    /// <param name="param">The parameter value to validate</param>
    /// <param name="maxSize">Specify '0' if maxSize should not be checked</param>
    /// <param name="paramName">The name of the parameter to validate</param>
    internal static void CheckPasswordParameter(ref string param, int maxSize, string paramName)
    {
      CheckParameter(ref param, true, true, false, maxSize, paramName);
    }

    /// <summary>
    /// Throws exception if 'param' is Null, Empty, contains commas, or is too big
    /// </summary>
    /// <param name="param">The parameter value to validate</param>
    /// <param name="checkForNull">If true, validates that param is not null</param>
    /// <param name="checkIfEmpty">If true, validates that param is not blank</param>
    /// <param name="checkForCommas">If true, validates that param does not contain one or more commas</param>
    /// <param name="maxSize">Specify '0' if maxSize should not be checked</param>
    /// <param name="paramName">The name of the parameter to validate</param>
    internal static void CheckParameter(ref string param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
    {
      if (null == param)
      {
        if (checkForNull)
          throw new ArgumentNullException(paramName);
        else
          return;
      }

      param = param.Trim();
      if (checkIfEmpty && param.Length < 1)
      {
        throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, StringResources.Parameter_can_not_be_empty, paramName), paramName);
      }

      if (maxSize > 0 && param.Length > maxSize)
      {
        throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, StringResources.Parameter_too_long, paramName, maxSize.ToString(CultureInfo.InvariantCulture)), paramName);
      }

      if (checkForCommas && param.Contains(","))
      {
        throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, StringResources.Parameter_can_not_contain_comma, paramName), paramName);
      }
    }

    /// <summary>
    /// Throws exception if 'param' is Null or Empty or if it contains any values that are empty, contain commas, or are too big
    /// </summary>
    /// <param name="param">The array of strings to validate</param>
    /// <param name="checkForNull">If true, validates that param values are not null</param>
    /// <param name="checkIfEmpty">If true, validates that param values are not blank</param>
    /// <param name="checkForCommas">If true, validates that param values do not contain one or more commas</param>
    /// <param name="maxSize">Specify '0' if maxSize should not be checked</param>
    /// <param name="paramName">The name of the parameter to validate</param>
    internal static void CheckArrayParameter(ref string[] param, bool checkForNull, bool checkIfEmpty, bool checkForCommas, int maxSize, string paramName)
    {
      if (param == null)
      {
        throw new ArgumentNullException(paramName);
      }

      if (param.Length < 1)
      {
        throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, StringResources.Parameter_array_empty, paramName), paramName);
      }

      //Create a hashtable to help determine if there are duplicate values in the array of strings
      Hashtable values = new Hashtable(param.Length);
      for (int i = param.Length - 1; i >= 0; i--)
      {
        SecurityUtility.CheckParameter(ref param[i], checkForNull, checkIfEmpty, checkForCommas, maxSize, paramName + "[ " + i.ToString(CultureInfo.InvariantCulture) + " ]");
        if (values.Contains(param[i]))
        {
          throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, StringResources.Parameter_duplicate_array_element, paramName), paramName);
        }
        else
        {
          values.Add(param[i], param[i]);
        }
      }
    }

    /// <summary>
    /// Returns a Boolean value found in the NameValueCollection passed-in based on the valueName
    /// </summary>
    /// <param name="config"></param>
    /// <param name="valueName"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    internal static bool GetBooleanValue(NameValueCollection config, string valueName, bool defaultValue)
    {
      //Get the string value from the NameValueCollection based on the valueName
      string stringValue = config[valueName];
      if (null == stringValue)
      {
        return defaultValue;
      }

      //Try and parse the string as "true" or "false"
      bool result;
      if (bool.TryParse(stringValue, out result))
      {
        return result;
      }
      else
      {
        throw new ProviderException(String.Format(CultureInfo.CurrentCulture, StringResources.Value_must_be_boolean, valueName));
      }
    }

    /// <summary>
    /// Returns an Integer value found in the NameValueCollection passed-in based on the valueName
    /// </summary>
    /// <param name="config"></param>
    /// <param name="valueName"></param>
    /// <param name="defaultValue"></param>
    /// <param name="zeroAllowed"></param>
    /// <param name="maxValueAllowed"></param>
    /// <returns></returns>
    internal static int GetIntValue(NameValueCollection config, string valueName, int defaultValue, bool zeroAllowed, int maxValueAllowed)
    {
      string stringValue = config[valueName];

      if (null == stringValue)
      {
        return defaultValue;
      }

      int intValue;
      if (!Int32.TryParse(stringValue, out intValue))
      {
        if (zeroAllowed)
        {
          throw new ProviderException(String.Format(CultureInfo.CurrentCulture, StringResources.Value_must_be_non_negative_integer, valueName));
        }

        throw new ProviderException(String.Format(CultureInfo.CurrentCulture, StringResources.Value_must_be_positive_integer, valueName));
      }

      if (zeroAllowed && intValue < 0)
      {
        throw new ProviderException(String.Format(CultureInfo.CurrentCulture, StringResources.Value_must_be_non_negative_integer, valueName));
      }

      if (!zeroAllowed && intValue <= 0)
      {
        throw new ProviderException(String.Format(CultureInfo.CurrentCulture, StringResources.Value_must_be_positive_integer, valueName));
      }

      if (maxValueAllowed > 0 && intValue > maxValueAllowed)
      {
        throw new ProviderException(String.Format(CultureInfo.CurrentCulture, StringResources.Value_too_big, valueName, maxValueAllowed.ToString(CultureInfo.InvariantCulture)));
      }

      return intValue;
    }

    #endregion
  }
}
