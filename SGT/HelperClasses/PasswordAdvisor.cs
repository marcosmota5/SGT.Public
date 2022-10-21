using System.Text.RegularExpressions;

namespace SGT.HelperClasses
{
    public class PasswordAdvisor
    {
        public enum PasswordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

        public static PasswordScore CheckStrength(string password)
        {
            int score = 0;

            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;
            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.Match(password, "[0-9]").Success)
                score++;
            if (Regex.Match(password, "[a-z]").Success &&
              Regex.Match(password, "[A-Z]").Success)
                score++;
            if (Regex.Match(password, "[^a-zA-Z0-9]").Success)
                score++;

            return (PasswordScore)score;
        }
    }
}