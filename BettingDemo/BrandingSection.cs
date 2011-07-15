using System;
using System.Configuration;
using System.Reflection;

namespace BettingDemo
{
    public class BrandingConfiguration : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get{ return (string) this["Name"]; }
            set { this["Name"] = value;}
        }

        [ConfigurationProperty("LogoUrl", IsRequired=true)]
        public string LogoUrl
        {
            get { return (string)this["LogoUrl"]; }
            set { this["LogoUrl"] = value; }            
        } // LogoUrl
        //------------

        [ConfigurationProperty("LogoAlternateText", IsRequired = true)]
        public string LogoAlternateText
        {
            get { return (string)this["LogoAlternateText"]; }
            set { this["LogoAlternateText"] = value; }
        } // LogoUrl
        //------------

        [ConfigurationProperty("StyleSheetUrl", IsRequired = true)]
        public string StyleSheetUrl
        {
            get { return (string)this["StyleSheetUrl"]; }
            set { this["StyleSheetUrl"] = value; }            
        }

    }

    public class BrandingConfigurationCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new BrandingConfiguration();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((BrandingConfiguration)element).Name;
        }

    } // class BrandingConfigurationCollection
    //---------------------------

    public class BrandingConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("brands")]
        public BrandingConfigurationCollection Brands
        {
            get { return (BrandingConfigurationCollection) this["brands"];  }
        }

    } // class BrandingConfigurationSection

    /**
     * Helper class to return attributes based on brand selected
     */
    public class Branding
    {
        public static string GetProperty(string propertyName)
        {
            string brandingName = ConfigurationManager.AppSettings["Branding"];
            BrandingConfiguration brandingConfiguration = GetByName(brandingName);
            PropertyInfo propertyInformation = brandingConfiguration.GetType().GetProperty(propertyName);

            return (string)propertyInformation.GetValue(brandingConfiguration, null);
            
        } // GetProperty ()
        //------------------

        public static string LogoUrl ()
        {
            return GetProperty("LogoUrl");
        } // Logo ()
        //------------

        public static string LogoAlternateText ()
        {
            return GetProperty("LogoAlternateText");
        }

        public static string StyleSheetUrl ()
        {
            return GetProperty("StyleSheetUrl");
        }

        public static BrandingConfiguration GetByName (string i_brandingName)
        {
            BrandingConfigurationSection configSection = null;
            configSection = ConfigurationManager.GetSection("BrandConfigurations") as BrandingConfigurationSection;
            foreach (var brand in configSection.Brands)
            {
                if ( ((BrandingConfiguration)brand).Name == i_brandingName )
                {
                    return (BrandingConfiguration)brand;
                }
            }
            return null;
        } // GetByName
        //-------------

    } // class Branding
    //-------------------
} // BettingDemo
//----------------