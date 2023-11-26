using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace C969
{
    static class Languages
    {
        static Dictionary<string, Dictionary<string, string>> languageDictionary = new Dictionary<string, Dictionary<string, string>>();

        static Languages()
        {
            Dictionary<string, string> appStrings = new Dictionary<string, string>();
            appStrings.Add("$languagetag", "Language: English");
            appStrings.Add("$please", "please");
            appStrings.Add("$login", "log in");
            appStrings.Add("$username", "username");
            appStrings.Add("$consultant", "consultant");
            appStrings.Add("$password", "password");
            appStrings.Add("$wrongpassword", "password is not valid");
            appStrings.Add("$usernamenotfound", "username was not found");
            appStrings.Add("$usertag", "logged on user:");
            appStrings.Add("$calendartab", "calendar");
            appStrings.Add("$appointmentlisttab", "appointment list");
            appStrings.Add("$customertab", "customer Record Informaton");
            appStrings.Add("$mainform", "main form");
            appStrings.Add("$addappointment", "new appointment");
            appStrings.Add("$removeappointment", "remove appointment");
            appStrings.Add("$add", "add");
            appStrings.Add("$edit", "edit");
            appStrings.Add("$remove", "remove");
            appStrings.Add("$customer", "customer");
            appStrings.Add("$refresh", "refresh");
            appStrings.Add("$day", "day");
            appStrings.Add("$week", "week");
            appStrings.Add("$month", "month");
            appStrings.Add("$name", "name");
            appStrings.Add("$addressLine1", "address");
            appStrings.Add("$addressLine2", "address (continued)");
            appStrings.Add("$zipcode", "zipcode");
            appStrings.Add("$phone", "phone");
            appStrings.Add("$save", "save");
            appStrings.Add("$cancel", "cancel");
            appStrings.Add("$city", "city");
            appStrings.Add("$country", "country");
            appStrings.Add("$notfound", "not found");
            appStrings.Add("$customerinformation", "customer information");
            appStrings.Add("$cannotdelete", "unable to delete from database:");
            appStrings.Add("$cannotread", "unable to read from database:");
            appStrings.Add("$cannotset", "unable to set");
            appStrings.Add("$reportstab", "reports");
            appStrings.Add("$#appointmenttypebymonth", "number of appointment types by month");
            appStrings.Add("$appointmenttype", "appointment Type");
            appStrings.Add("$numberofappointments", "number of appointments");
            appStrings.Add("$errorrunningreport", "error running report:");
            appStrings.Add("$toomanycolumns", "too many columns were included on input query");
            appStrings.Add("$scheduleforeachconsultant", "the schedule for each consultant");
            appStrings.Add("$myscheduledhourspermonth", "my scheduled hours per month");
            appStrings.Add("$hours", "hours");
            appStrings.Add("$logfileerror", "An error was encountered writing to the log file. The application will shut down.");
            appStrings.Add("$usetesttest", "Login information not provided, use username:test, and password: test");
            appStrings.Add("$userloggedin", "user logged in");
            appStrings.Add("$launchtextfile", "launch text file");
            appStrings.Add("$upcomingappointmentalert", "you have an upcoming appointment:");
            appStrings.Add("$upcomingappointment", "upcoming appointment");
            appStrings.Add("$entercustomername", "please enter the client name");
            appStrings.Add("$enteraddress", "please enter a valid address");
            appStrings.Add("$enterphone", "please enter a valid phone");
            appStrings.Add("$invalidphonecharacters", "phone number contains invalid characters");
            appStrings.Add("$enterzipcode", "please enter a zip code");
            appStrings.Add("$entercity", "please select or enter a valid city");
            appStrings.Add("$entercountry", "please select or enter a valid country");
            appStrings.Add("$confirmdelete", "confirm deletion of this");
            appStrings.Add("$confirm", "please confirm");
            appStrings.Add("$selectcustomer", "select a customer");
            appStrings.Add("$creatingcustomer", "creating customer object");
            appStrings.Add("$logoff", "Log Off");
            appStrings.Add("$showby", "show by");
            appStrings.Add("$showfor", "show for");
            appStrings.Add("$me", "me");
            appStrings.Add("$everyone", "everyone");
            appStrings.Add("$type", "type");
            appStrings.Add("$start", "start");
            appStrings.Add("$end", "end");
            appStrings.Add("$selectacustomer", "select a customer");
            appStrings.Add("$appointment", "appointment");
            appStrings.Add("$selectaconsultant", "select a consultant");
            appStrings.Add("$description", "description");
            appStrings.Add("$location", "location");
            appStrings.Add("$appointmentconflict", "this appointment time conflicts with an existing appointment");
            appStrings.Add("$duration", "duration");
            appStrings.Add("$minutes", "minutes");
            appStrings.Add("$appointmentoutsidebusinesshours", "you must schedule this appointment during business hours, between 6am and 6pm.");
            appStrings.Add("$contact", "contact");
            appStrings.Add("$selectappointment", "select an appointment");
            appStrings.Add("$time", "time");
            appStrings.Add("$reportname", "report name");
            languageDictionary.Add("en", appStrings);

            appStrings = new Dictionary<string, string>();
            appStrings.Add("$please", "bitte");
            appStrings.Add("$login", "einloggen");
            appStrings.Add("$username", "nutzername");
            appStrings.Add("$consultant", "berater");
            appStrings.Add("$password", "passwort");
            appStrings.Add("$wrongpassword", "passwort ist ungültig"); appStrings.Add("$usernamenotfound", "benutzername wurde nicht gefunden");
            appStrings.Add("$usertag", "Angemeldeter Benutzer:");
            appStrings.Add("$calendartab", "kalendar"); appStrings.Add("$appointmentlisttab", "Terminliste"); appStrings.Add("$customertab", "Kundendatensatz-Informationen"); appStrings.Add("$mainform", "Hauptformular"); appStrings.Add("$addappointment", "Neuer Termin"); appStrings.Add("$removeappointment", "Termin entfernen"); appStrings.Add("$add", "hinzufügen"); appStrings.Add("$edit", "redigieren"); appStrings.Add("$remove", "entfernen"); appStrings.Add("$customer", "Kunde"); appStrings.Add("$refresh", "auffrischen"); appStrings.Add("$day", "tag"); appStrings.Add("$week", "woche"); appStrings.Add("$month", "monat"); appStrings.Add("$name", "name"); appStrings.Add("$addressLine1", "adresse"); appStrings.Add("$addressLine2", "adresse (Fortsetzung)"); appStrings.Add("$zipcode", "Postleitzahl"); appStrings.Add("$phone", "telefon"); appStrings.Add("$save", "retten"); appStrings.Add("$cancel", "Abbrechen"); appStrings.Add("$city", "stadt"); appStrings.Add("$country", "land"); appStrings.Add("$notfound", "nicht gefunden"); appStrings.Add("$customerinformation", "Kundeninformation"); appStrings.Add("$cannotdelete", "Aus der Datenbank kann nicht gelöscht werden:"); appStrings.Add("$cannotread", "Aus der Datenbank kann nicht gelesen werden:"); appStrings.Add("$cannotset", "Es kann nicht festgelegt werden");
            appStrings.Add("$reportstab", "Berichte"); appStrings.Add("$#appointmenttypebymonth", "Anzahl der Terminarten pro Monat"); appStrings.Add("$appointmenttype", "Art des Termins"); appStrings.Add("$numberofappointments", "Anzahl der Termine"); appStrings.Add("$errorrunningreport", "Fehler beim Ausführen des Berichts:"); appStrings.Add("$toomanycolumns", "Zu viele Spalten wurden in der Eingabeabfrage enthalten"); appStrings.Add("$scheduleforeachconsultant", "Der Zeitplan für jeden Berater"); appStrings.Add("$myscheduledhourspermonth", "Meine geplanten Stunden pro Monat"); appStrings.Add("$hours", "Stunden"); appStrings.Add("$logfileerror", "Beim Schreiben in die Protokolldatei ist ein Fehler aufgetreten. Die Anwendung wird heruntergefahren."); appStrings.Add("$usetesttest", "Anmeldeinformationen nicht angegeben, verwenden Sie Benutzername:test und Kennwort: test"); appStrings.Add("$userloggedin", "Benutzer eingeloggt"); appStrings.Add("$launchtextfile", "Textdatei starten"); appStrings.Add("$upcomingappointmentalert", "Sie haben einen bevorstehenden Termin:"); appStrings.Add("$upcomingappointment", "Nächster Termin"); appStrings.Add("$entercustomername", "Bitte geben Sie den Namen des Kunden ein"); appStrings.Add("$enteraddress", "Bitte geben Sie eine gültige Adresse ein"); appStrings.Add("$enterphone", "Bitte geben Sie eine gültige Telefonnummer ein"); appStrings.Add("$invalidphonecharacters", "Telefonnummer enthält ungültige Zeichen");
            appStrings.Add("$enterzipcode", "Bitte geben Sie eine Postleitzahl ein");
            appStrings.Add("$entercity", "Bitte wählen Sie eine gültige Stadt aus oder geben Sie sie ein");
            appStrings.Add("$entercountry", "Bitte wählen Sie ein gültiges Land aus oder geben Sie es ein");
            appStrings.Add("$confirmdelete", "Bestätigen Sie die Löschung dieser");
            appStrings.Add("$confirm", "Bitte bestätigen Sie");
            appStrings.Add("$selectcustomer", "Wählen Sie einen Kunden aus");
            appStrings.Add("$creatingcustomer", "Erstellen eines Kundenobjekts");
            appStrings.Add("$logoff", "Ausloggen");
            appStrings.Add("$showby", "anzeigen von");
            appStrings.Add("$showfor", "anzeigen für");
            appStrings.Add("$me", "ich");
            appStrings.Add("$everyone", "jeder");
            appStrings.Add("$type", "Art");
            appStrings.Add("$start", "anfangen");
            appStrings.Add("$end", "ende");
            appStrings.Add("$selectacustomer", "Wählen Sie einen Kunden aus");
            appStrings.Add("$appointment", "Verabredung");
            appStrings.Add("$selectaconsultant", "Wählen Sie einen Berater aus");
            appStrings.Add("$description", "Beschreibung");
            appStrings.Add("$location", "Ort");
            appStrings.Add("$appointmentconflict", "Diese Terminzeit steht in Konflikt mit einem bestehenden Termin");
            appStrings.Add("$duration", "Dauer");
            appStrings.Add("$minutes", "Protokoll");
            appStrings.Add("$appointmentoutsidebusinesshours", "Sie müssen diesen Termin während der Geschäftszeiten zwischen 6 und 18 Uhr vereinbaren.");
            appStrings.Add("$contact", "Kontakt");
            appStrings.Add("$selectappointment", "Wählen Sie einen Termin aus");
            appStrings.Add("$time", "Zeit");
            appStrings.Add("$reportname", "Name des Berichts");
            languageDictionary.Add("de", appStrings);

            CultureInfo ci = CultureInfo.CurrentUICulture;

            if (!(languageDictionary.ContainsKey(ci.TwoLetterISOLanguageName)))
            {
                string supportedLanguageString = "";

                foreach (string s in languageDictionary.Keys)
                {
                    supportedLanguageString += "\n" + (new CultureInfo(s).DisplayName);
                }

                MessageBox.Show(ci.DisplayName + " is not supported. Dialogs will be shown in English.\n\nSupported Languages:" + supportedLanguageString);
            }
        }
        public static string LanguageFill(string s)
        {
            LanguageFill(ref s);
            return s;
        }
        public static void LanguageFill(ref string s)
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            string languageName = ci.TwoLetterISOLanguageName;
            if (!(languageDictionary.ContainsKey(languageName)))
                languageName = "en";

            ReplaceStrings(ref s, languageName);
        }
        public static void LanguageFill(Control.ControlCollection controls)
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            string languageName = ci.Name.Substring(0, 2);
            if (!(languageDictionary.ContainsKey(languageName)))
                languageName = "en";
            foreach (Control c in controls)
            {
                ProcessControl(c, languageName);
            }
        }
        public static void LanguageFill(ref Form f)
        {
            CultureInfo ci = CultureInfo.CurrentCulture;
            string languageName = ci.Name.Substring(0, 2);
            if (!(languageDictionary.ContainsKey(languageName)))
                languageName = "en";
            string appString = f.Text;
            if (ReplaceStrings(ref appString, languageName))
            {
                f.Text = ci.TextInfo.ToTitleCase(appString);
            }
            foreach (Control c in f.Controls)
            {
                ProcessControl(c, languageName);
            }
        }
        static void ProcessControl(Control c, string languageName)
        {
            string appString = "";
            appString = c.Text;
            if (ReplaceStrings(ref appString, languageName))
            {
                c.Text = appString;
            }
            if (c.HasChildren)
                foreach (Control child in c.Controls)
                    ProcessControl(child, languageName);
        }
        static bool ReplaceStrings(ref string appString, string languageName)
        {
            if (appString.Contains("$") || appString.Contains("$"))
            {
                string[] textSplit = appString.Split(' ');
                appString = "";
                foreach (string s in textSplit)
                {
                    if (s.StartsWith("$") && Session.GetVariable(s) != "")
                    {
                        appString += Session.GetVariable(s);
                    }
                    else if (s.StartsWith("$") && languageDictionary[languageName].ContainsKey(s.ToLower()))
                    {
                        appString += languageDictionary[languageName][s.ToLower()];
                    }
                    else
                    {
                        appString += s;
                    }
                    appString += " ";
                }
                appString = appString[0].ToString().ToUpper() + appString.Substring(1, appString.Length - 1);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
