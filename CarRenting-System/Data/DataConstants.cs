using System;

namespace CarRenting_System.Data
{
    public class DataConstants
    {
        public class User
        {
            public const int FullNameMinLength = 2;
            public const int FullNameMaxLength = 40;
        }
        public class Dealer
        {
            public const int FullNameMinLength = 2;
            public const int FullNameMaxLength = 30;

            public const int PhoneNumberMinLength = 5;
            public const int PhoneNumberMaxLength = 15;
        }
        public class Category
        {
            public const int NameMaxLength = 25;
        }
        public class Car
        {
            public const int MakeMinLength = 2;
            public const int MakeMaxLength = 20;

            public const int ModelMinLength = 2;
            public const int ModelMaxLength = 30;

            public const int DescriptionMinLength = 3;
            public const int DescriptionMaxLength = 3000;

            public const int YearMinValue = 1900;
            public const int YearMaxValue = 2050;
        }
        
    }
}
