using System;

namespace HelpDesk.DTO
{
    /// <summary>
    /// Пользователь
    /// </summary>
    public class UserDTO
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName
        {
            get
            {
                return String.Format("{0} {1} {2}", FM, IM, OT).Trim();
            }
        }

        /// <summary>
        /// E-mail (в качестве логина используется, может также только имя почтового ящика без домена)
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password { get; set; }
                
        /// <summary>
        /// Фамилия
        /// </summary>
        public string FM { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string IM { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string OT { get; set; }

    }
}
