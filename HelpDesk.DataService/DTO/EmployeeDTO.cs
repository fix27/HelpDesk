using HelpDesk.DataService.DTO.Resources;
using System;

namespace HelpDesk.DataService.DTO
{
    /// <summary>
    /// Личные данные пользователя
    /// </summary>
    public class EmployeeDTO
    {
        public long Id { get; set; }

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

        /// <summary>
        /// Организация
        /// </summary>
        public long? OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAddress { get; set; }
        /// <summary>
        /// Должность
        /// </summary>
        public string PostName { get; set; }

        /// <summary>
        /// Кабинет
        /// </summary>
        public string Cabinet { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Подписан на E-mail рассылку
        /// </summary>
        public bool Subscribe { get; set; }

        public static string GetEmployeeInfo(string fm, string im, string ot, string phone, string organizationName, string organizationAddress)
        {
            return String.Format("{0} {1} {2}, {3}: {4}, {5}, {6}", fm, im, ot, Resource.Name_PhoneShort, phone, organizationName, organizationAddress);
        }
        public string EmployeeInfo
        {
            get
            {
                return GetEmployeeInfo(FM, IM, OT, Phone, OrganizationName, OrganizationAddress);
            }
        }

        public string ShortEmployeeInfo
        {
            get
            {
                return String.Format("{0} {1} {2}, {3}: {4}", FM, IM, OT, Resource.Name_PhoneShort, Phone);
            }
        }

    }
}
