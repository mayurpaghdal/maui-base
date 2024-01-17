namespace MauiBase.Common;

public class AppCommon
{
    #region Conversion Methods

    #region Math

    #region ToByte Overloads

    #region ToByte(Object value)
    /// <summary>
    /// Method to return Byte equivalent of the passed value.
    /// </summary>
    /// <param name="value">Object value to be converted.</param>
    /// <returns>Byte</returns>
    public static Byte ToByte(Object value)
    {
        Byte returnValue = 0;
        try
        {
            // Check null and DBNull, because method return 0 value.
            // So does not call method for DBNull.Value.
            if (value != null &&
                value != DBNull.Value)
            {
                returnValue = ToByte(value.ToString());
            }
        }
        catch { }

        return returnValue;
    }
    #endregion

    #region ToByte(String value)
    /// <summary>
    /// Method to return Byte equivalent of the passed value.
    /// </summary>
    /// <param name="value">String value to be converted.</param>
    /// <returns>Byte</returns>
    public static Byte ToByte(String value)
    {
        Byte returnValue = 0;
        try
        {
            // Converts the string representation of a number to its byte equivalent.
            Byte.TryParse(value,
                          out returnValue);
        }
        catch { }

        return returnValue;
    }
    #endregion

    #endregion

    #region ToInt16 Overloads

    #region ToInt16(Object value)
    /// <summary>
    /// Method to return Int16 equivalent of the passed value.
    /// </summary>
    /// <param name="value">Object value to be converted.</param>
    /// <returns>Int16</returns>
    public static Int16 ToInt16(Object value)
    {
        Int16 returnValue = 0;
        try
        {
            // Check null and DBNull, because method return 0 value.
            // So does not call method for DBNull.Value.
            if (value != null &&
                value != DBNull.Value)
            {
                returnValue = ToInt16(value.ToString());
            }
        }
        catch { }
        return returnValue;
    }
    #endregion

    #region ToInt16(String value)
    /// <summary>
    /// Method to return Int16 equivalent of the passed value.
    /// </summary>
    /// <param name="value">String value to be converted.</param>
    /// <returns>Int16</returns>
    public static Int16 ToInt16(String value)
    {
        Int16 returnValue = 0;
        try
        {
            // Converts the string representation of a number to its 16-bit signed integer
            // equivalent.
            Int16.TryParse(value,
                           out returnValue);
        }
        catch { }
        return returnValue;
    }
    #endregion

    #endregion

    #region ToInt32 Overloads

    #region ToInt32(Object value)
    /// <summary>
    /// Method to return Int32 equivalent of the passed value.
    /// </summary>
    /// <param name="value">Object value to be converted.</param>
    /// <returns>
    /// A 32-bit signed integer equivalent to the value of value.-or- Zero if value is null.
    /// </returns>
    public static Int32 ToInt32(Object value)
    {
        Int32 returnValue = 0;
        try
        {
            // Check null and DBNull, because method return 0 value.
            // So does not call method for DBNull.Value.
            if (value != null &&
                value != DBNull.Value)
            {
                returnValue = ToInt32(value.ToString());
            }
        }
        catch { }
        return returnValue;
    }
    #endregion

    #region ToInt32(String value)
    /// <summary>
    /// Method to return Int32 equivalent of the passed value.
    /// </summary>
    /// <param name="value">String value to be converted.</param>
    /// <returns>
    /// A 32-bit signed integer equivalent to the value of value.-or- Zero if value is null.
    /// </returns>
    public static Int32 ToInt32(String value)
    {
        int returnValue = 0;
        try
        {
            // Converts the string representation of a number to its 32-bit signed integer
            // equivalent.
            int.TryParse(value,
                         out returnValue);
        }
        catch { }
        return returnValue;
    }
    #endregion

    #endregion

    #region ToInt64 Overloads

    #region ToInt64(Object value)
    /// <summary>
    /// Method to return Int64 equivalent of the passed value.
    /// </summary>
    /// <param name="value">Object value to be converted.</param>
    /// <returns>
    /// A 64-bit signed integer equivalent to the value of value.-or- Zero if value is null.
    /// </returns>
    public static Int64 ToInt64(Object value)
    {
        Int64 returnValue = 0;
        try
        {
            // Check null and DBNull, because method return 0 value.
            // So does not call method for DBNull.Value.
            if (value != null &&
                value != DBNull.Value)
            {
                returnValue = ToInt64(value.ToString());
            }
        }
        catch { }
        return returnValue;
    }
    #endregion

    #region ToInt64(String value)
    /// <summary>
    /// Method to return Int64 equivalent of the passed value.
    /// </summary>
    /// <param name="value">String value to be converted.</param>
    /// <returns>
    /// A 64-bit signed integer equivalent to the value of value.-or- Zero if value is null.
    /// </returns>
    public static Int64 ToInt64(String value)
    {
        Int64 returnValue = 0;
        try
        {
            // Converts the string representation of a number to its 64-bit signed integer
            // equivalent.
            Int64.TryParse(value,
                           out returnValue);
        }
        catch { }
        return returnValue;
    }
    #endregion

    #endregion

    #region ToDecimal Overloads

    #region ToDecimal(Object value)
    /// <summary>
    /// Method to return Decimal equivalent of the passed value.
    /// </summary>
    /// <param name="value">Object value to be converted.</param>
    /// <returns>
    /// A System.Decimal number equivalent to the value of value, or zero if value
    /// is null.
    /// </returns>
    public static Decimal ToDecimal(Object value)
    {
        decimal returnValue = 0;
        try
        {
            // Check null and DBNull, because method return 0 value.
            // So does not call method for DBNull.Value.
            if (value != null &&
                value != DBNull.Value)
            {
                returnValue = ToDecimal(value.ToString());
            }
        }
        catch { }

        return returnValue;
    }
    #endregion

    #region ToDecimal(String value)
    /// <summary>
    /// Method to return Decimal equivalent of the passed value.
    /// </summary>
    /// <param name="value">String value to be converted.</param>
    /// <returns>
    /// A System.Decimal number equivalent to the value of value, or zero if value
    /// is null.
    /// </returns>
    public static Decimal ToDecimal(String value)
    {
        decimal returnValue = 0;
        try
        {
            // Converts the string representation of a number to its System.Decimal equivalent.
            decimal.TryParse(value,
                             out returnValue);
        }
        catch { }
        return returnValue;
    }
    #endregion

    #endregion

    #region ToDouble Overloads

    #region ToDouble(Object value)
    /// <summary>
    /// Method to return Double equivalent of the passed value.
    /// </summary>
    /// <param name="value">Object value to be converted.</param>
    /// <returns>
    /// A double-precision floating point number equivalent to the value of value.-or-
    /// Zero if value is null.
    /// </returns>
    public static Double ToDouble(Object value)
    {
        Double returnValue = 0;
        try
        {
            // Check null and DBNull, because method return 0 value.
            // So does not call method for DBNull.Value.
            if (value != null &&
                value != DBNull.Value)
            {
                returnValue = ToDouble(value.ToString());
            }
        }
        catch { }
        return returnValue;
    }
    #endregion

    #region ToDouble(String value)
    /// <summary>
    /// Method to return Double equivalent of the passed value.
    /// </summary>
    /// <param name="value">String value to be converted.</param>
    /// <returns>
    /// A double-precision floating point number equivalent to the value of value.-or-
    /// Zero if value is null.
    /// </returns>
    public static Double ToDouble(String value)
    {
        double returnValue = 0;
        try
        {
            // Converts the string representation of a number to its System.Double equivalent.
            double.TryParse(value,
                             out returnValue);
        }
        catch { }

        return returnValue;
    }
    #endregion

    #endregion

    #region ToSingle Overloads

    #region ToSingle(Object value)
    /// <summary>
    /// Method to return Single equivalent of the passed value.
    /// </summary>
    /// <param name="value">Object value to be converted.</param>
    /// <returns>
    /// A single-precision floating point number equivalent to the value of value.-or-
    /// Zero if value is null.
    /// </returns>
    public static Single ToSingle(Object value)
    {
        float returnValue = 0;
        try
        {
            // Check null and DBNull, because method return 0 value.
            // So does not call method for DBNull.Value.
            if (value != null &&
                value != DBNull.Value)
            {
                returnValue = ToSingle(value.ToString());
            }
        }
        catch { }
        return returnValue;
    }
    #endregion

    #region ToSingle(String value)
    /// <summary>
    /// Method to return Single equivalent of the passed value.
    /// </summary>
    /// <param name="value">String value to be converted.</param>
    /// <returns>
    /// A single-precision floating point number equivalent to the value of value.-or-
    /// Zero if value is null.
    /// </returns>
    public static Single ToSingle(String value)
    {
        float returnValue = 0;
        try
        {
            // Converts the string representation of a number to its System.Single equivalent.
            float.TryParse(value,
                           out returnValue);
        }
        catch { }

        return returnValue;
    }
    #endregion

    #endregion

    #region ToBoolean Overloads

    #region ToBoolean(Object value)
    /// <summary>
    /// Method to return Boolean equivalent of the passed value.
    /// </summary>
    /// <param name="value">Object value to be converted.</param>
    /// <returns>Boolean</returns>
    public static Boolean ToBoolean(Object value)
    {
        Boolean returnValue = false;
        try
        {
            // Check null and DBNull, because method return false value.
            // So does not call method for DBNull.Value.
            if (value != null &&
                value != DBNull.Value)
            {
                returnValue = ToBoolean(value.ToString());
            }
        }
        catch { }

        return returnValue;
    }
    #endregion

    #region ToBoolean(String value)
    /// <summary>
    /// Method to return Boolean equivalent of the passed value.
    /// </summary>
    /// <param name="value">String value to be converted.</param>
    /// <returns>Boolean</returns>
    public static Boolean ToBoolean(String value)
    {
        Boolean returnValue = false;
        try
        {
            if (string.IsNullOrEmpty(value) == false &&
                (value == "1" ||
                 value.Equals(Boolean.TrueString, StringComparison.CurrentCultureIgnoreCase) == true))
            {
                returnValue = true;
            }
        }
        catch { }
        return returnValue;
    }
    #endregion

    #endregion

    #region ToDateTime Overloads

    #region ToDateTime(Object value)
    /// <summary>
    /// Method to return DateTime equivalent of the passed value.
    /// </summary>
    /// <param name="value">Object value to be converted.</param>
    /// <returns>DateTime</returns>
    public static DateTime ToDateTime(Object value)
    {
        DateTime returnValue = DateTime.Now;
        try
        {
            if (value != null)
            {
                returnValue = Convert.ToDateTime(value);
            }
        }
        catch { }

        return returnValue;
    }
    #endregion

    #region ToDateTime(String value)
    /// <summary>
    /// Method to return DateTime equivalent of the passed value.
    /// </summary>
    /// <param name="value">String value to be converted.</param>
    /// <returns>DateTime</returns>
    public static DateTime ToDateTime(String value)
    {
        DateTime returnValue = DateTime.Now;
        try
        {
            if (string.IsNullOrEmpty(value) == false)
            {
                returnValue = Convert.ToDateTime(value);
            }
        }
        catch { }

        return returnValue;
    }
    #endregion

    #endregion

    #region Round
    /// <summary>
    /// Method to return Double value rounded to specified decimal places.
    /// </summary>
    /// <param name="value">Double value to be rounded.</param>
    /// <param name="decimalPlaces">Decimal places to be rounded.</param>
    /// <returns>Double</returns>
    public static Double Round(Double value,
                               Int32 decimalPlaces)
    {
        Double returnValue = 0;
        String strValue = String.Empty;

        try
        {
            //Get the string equivalent with the said decimal places.
            if (decimalPlaces <= 0) { strValue = value.ToString("0"); }
            else { strValue = value.ToString("0." + "0".PadRight(decimalPlaces, '0')); }

            //Convert back to double value;
            returnValue = ToDouble(strValue);
        }
        catch { }

        return returnValue;
    }
    #endregion

    #region RoundInMultipleOf
    /// <summary>
    /// Method to get the input value rounded in multiple of a double.
    /// </summary>
    /// <param name="valueToRound">Value to be rounded.</param>
    /// <param name="inMultipleOf">Multiplier of the value.</param>
    /// <returns>Zero if any exception occurs else the rounded value.</returns>
    public static Double RoundInMultipleOf(Double valueToRound,
                                           Double inMultipleOf)
    {
        Double roundedValue = 0;

        try
        {
            //Round the value as per multiple of.
            roundedValue =
              RoundInMultipleOf(valueToRound,
                                inMultipleOf,
                                false);
        }
        catch
        { throw; }

        return roundedValue;
    }
    #endregion

    #region RoundInMultipleOf
    /// <summary>
    /// Method to get the input value rounded in multiple of a double.
    /// </summary>
    /// <param name="valueToRound">Value to be rounded.</param>
    /// <param name="inMultipleOf">Multiplier of the value.</param>
    /// <param name="forceCeiling">Pass true if you want only celling value(higher) otherwise false.</param>
    /// <returns>Zero if any exception occurs else the rounded value.</returns>
    public static Double RoundInMultipleOf(Double valueToRound,
                                           Double inMultipleOf,
                                           Boolean forceCeiling)
    {
        Double roundedValue = 0;

        try
        {
            if (valueToRound == 0 ||
                inMultipleOf == 0)
            {
                //return 0 if either the value or multiplier is 0.
                roundedValue = 0;
            }
            else
            {
                //Round the value as per multiple of.
                roundedValue =
                  forceCeiling == false && (valueToRound % inMultipleOf) < (inMultipleOf / 2) ?
                    Math.Floor(valueToRound / inMultipleOf) * inMultipleOf :
                    Math.Ceiling(valueToRound / inMultipleOf) * inMultipleOf;
            }
        }
        catch
        { throw; }

        return roundedValue;
    }
    #endregion

    #endregion

    #region NumToEnum<T>
    /// <summary>
    /// Method to convert a number to an enum.
    /// </summary>
    /// <typeparam name="T">DataType of enum.</typeparam>
    /// <param name="number">Number value to be converted to an equivalent enum value.</param>
    /// <returns>Datatype of enum.</returns>
    public static T NumToEnum<T>(int number) => (T)System.Enum.ToObject(typeof(T), number);
    #endregion

    #region NumberToWords
    /// <summary>
    /// Converts an numeric value into English words.
    /// Safe Max. value being 1000000000 with any number of decimal places.
    /// </summary>
    /// <param name="number">Number to be converted.</param>
    /// <returns>string</returns>
    public static String NumberToWords(Double number)
    {
        //Initialise the return text.
        StringBuilder rtnValue = new StringBuilder(String.Empty);
        //Set the max. valid value.
        Int32 maxValidValue = 1000000000;

        //Check for valid range.
        if (number < maxValidValue)
        {
            try
            {
                #region Single-digit and small number names
                String[] zero2Nine = new String[]
                  {"Zero ",
         "One ",
         "Two ",
         "Three ",
         "Four ",
         "Five ",
         "Six ",
         "Seven ",
         "Eight ",
         "Nine "};
                #endregion

                #region Scale constants
                const String HUNDRED = "Hundred ";
                const String THOUSAND = "Thousand ";
                const String LAC = "Lac ";
                const String CRORE = "Crore ";
                #endregion

                #region Store the digits in the number in an int array
                String strNumber = Math.Abs(number).ToString();
                Int32 lenStrNumber = strNumber.Length;
                Int32 decimalIndex = strNumber.IndexOf('.');
                Boolean isDecimalNumber = true;

                //Reset the string if the number is a whole number.
                if (decimalIndex < 0)
                {
                    strNumber += ".0";
                    lenStrNumber += 2;
                    decimalIndex = lenStrNumber - 2;
                    isDecimalNumber = false;
                }

                Int32[] digitsLeftToDecimal = new Int32[9];
                Int32[] digitsRightToDecimal = new Int32[lenStrNumber - decimalIndex - 1];

                //Loop through the string to store the digits left to decimal.
                for (Int32 idx = 0, i = decimalIndex - 1; i >= 0; idx++, i--)
                {
                    digitsLeftToDecimal[idx] = ToInt32(strNumber[i].ToString());
                }

                //Loop through the string to store the digits right to decimal.
                for (Int32 idx = 0, j = decimalIndex + 1; j < lenStrNumber; idx++, j++)
                {
                    digitsRightToDecimal[idx] = ToInt32(strNumber[j].ToString());
                }

                #endregion

                #region Generate the words
                String words = String.Empty;

                #region Crore
                words = TwoDigitGroupToWords(digitsLeftToDecimal[8],
                                             digitsLeftToDecimal[7]);

                //Check for null or Empty.
                if (!String.IsNullOrEmpty(words))
                {
                    rtnValue.Append(words + CRORE);
                }
                #endregion

                #region Lac
                words = TwoDigitGroupToWords(digitsLeftToDecimal[6],
                                             digitsLeftToDecimal[5]);

                //Check for null or Empty.
                if (!String.IsNullOrEmpty(words))
                {
                    rtnValue.Append(words + LAC);
                }
                #endregion

                #region Thousand
                words = TwoDigitGroupToWords(digitsLeftToDecimal[4],
                                             digitsLeftToDecimal[3]);

                //Check for null or Empty.
                if (!String.IsNullOrEmpty(words))
                {
                    rtnValue.Append(words + THOUSAND);
                }
                #endregion

                #region Hundred
                if (digitsLeftToDecimal[2] != 0)
                {
                    rtnValue.Append(zero2Nine[digitsLeftToDecimal[2]] + HUNDRED);
                }
                #endregion

                #region Tens and Units
                words = TwoDigitGroupToWords(digitsLeftToDecimal[1],
                                             digitsLeftToDecimal[0]);

                //Check for null or Empty.
                if (!String.IsNullOrEmpty(words))
                {
                    if (isDecimalNumber || String.IsNullOrEmpty(rtnValue.ToString())) { rtnValue.Append(words); }
                    else { rtnValue.Append("and " + words); }
                }
                #endregion

                #region Decimals
                if (isDecimalNumber)
                {
                    //Append 'and'.
                    if (!String.IsNullOrEmpty(rtnValue.ToString())) { rtnValue.Append("and "); }

                    //Loop through the decimal part of the number.
                    for (int i = 0; i < digitsRightToDecimal.Length; i++)
                    {
                        rtnValue.Append(zero2Nine[digitsRightToDecimal[i]]);
                    }
                }
                #endregion

                #region Negative rule
                if (number < 0) { rtnValue.Insert(0, "Negative "); }
                #endregion

                #endregion
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        return rtnValue.ToString();
    }
    #endregion

    #region String

    #region ToProperCase(String Input)
    /// <summary>
    /// Provides method to convert to A System.String into Proper Case.
    /// </summary>
    /// <param name="value">A System.String reference.</param>
    /// <returns>A Proper Case System.String.</returns>
    public static String ToProperCase(String value)
    {
        try
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    #endregion

    #region ToString(Object value)
    /// <summary>
    /// Converts the value of the specified DataType to its System.String representation.
    /// </summary>
    /// <param name="value">The value to be convert into System.String.</param>
    /// <returns>The System.String equivalent of the value of value.</returns>
    public static String ToString(Object value)
    {
        string returnValue = string.Empty;
        try
        {
            if (value != null &&
                value != DBNull.Value)
            {
                returnValue = Convert.ToString(value);
            }
        }
        catch { }

        return returnValue;
    }
    #endregion

    #endregion

    #region TwoDigitGroupToWords
    /// <summary>
    /// Converts a two-digit group into English words.
    /// </summary>
    /// <param name="leftDigit">Left digit of the number.</param>
    /// <param name="rightDigit">Right digit of the number.</param>
    /// <returns>string</returns>
    private static String TwoDigitGroupToWords(Int32 leftDigit,
                                               Int32 rightDigit)
    {
        //Initialise the return text.
        StringBuilder rtnValue = new StringBuilder(String.Empty);

        #region Declare and initialize the number names

        //Single-digit and small number names.
        String[] zero2Nine = new String[]
            {String.Empty,
       "One ",
       "Two ",
       "Three ",
       "Four ",
       "Five ",
       "Six ",
       "Seven ",
       "Eight ",
       "Nine "};

        //Numbers from 10 to 19.
        String[] ten2Nineteen = new String[]
            {"Ten ",
       "Eleven ",
       "Twelve ",
       "Thirteen ",
       "Fourteen ",
       "Fifteen ",
       "Sixteen ",
       "Seventeen ",
       "Eighteen ",
       "Nineteen "};

        //Tens number names from twenty upwards.
        String[] tyWords = new String[]
            {String.Empty,
       String.Empty,
       "Twenty ",
       "Thirty ",
       "Forty ",
       "Fifty ",
       "Sixty ",
       "Seventy ",
       "Eighty ",
       "Ninety "};
        #endregion

        try
        {
            //Generate word.
            if (leftDigit == 0)
            {
                rtnValue.Append(zero2Nine[rightDigit]);
            }
            else if (leftDigit == 1)
            {
                rtnValue.Append(ten2Nineteen[rightDigit]);
            }
            else
            {
                //Append word for the left digit.
                rtnValue.Append(tyWords[leftDigit]);

                //Append word for the right digit.
                rtnValue.Append(zero2Nine[rightDigit]);
            }


        }
        catch (Exception ex)
        {
            throw ex;
        }

        return rtnValue.ToString();
    }
    #endregion

    #endregion
}
