using System;
using System.ComponentModel.DataAnnotations;

namespace Pooyan.test.project.StateManagement.States
{
    public class BusinessState
    {
        public Guid? Id { get; set; }
        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string Telephone { get; set; }
        public Guid CityId { get; set; }

        private Guid _provinceId;
        public Guid ProvinceId
        {
            get => _provinceId;
            set
            {
                CityId = Guid.Empty;
                _provinceId = value;
            }
        }
        public int Sort { get; set; }
        public string JobExplanation { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string GoBetweenFirstName { get; set; }
        public string GoBetweenLastName { get; set; }
        public string GoBetweenTel { get; set; }
    }
}
