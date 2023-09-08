using barber_shop.Models.Enums;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace barber_shop.Models
{
    public class Person
    {
        [JsonIgnore]
        public int Id { get; set; }
        //campo obrigtório
        [Required]
        //msg de erro personalizada
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O tamanho do {0} deve ser entre {2} e {1}")]
        [Display(Name = "Nome")]
        public string Name { get; set; }
        [Required]
        [StringLength(11, ErrorMessage = "O {0} deve conter {1} caracteres")]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }
        [Column("id_gender")]
        public int GenderId { get; set; }
        [Display(Name = "Genêro")]
        public Gender Gender { get; set; }
        [Column("id_address")]
        public int AddressId { get; set; }
        public Address Address { get; set; }
        [Required]
        [StringLength(11, ErrorMessage = "O {0} deve conter {1} caracteres")]
        [Display(Name = "Telefone")]
        public string Telephone { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data Nascimento")]
        public DateTime BirthDate { get; set; }


        public bool ValidateCpf()
        {
            try
            {
                if (Cpf.Length != 11 || AvoidSequence(Cpf))
                {
                    return false;
                }

                string newCpf = Cpf.Substring(0, 9);
                int total = 0;
                int reverse = 10;
                int sequence = 27;

                for (int i = 0; i <= sequence; i++)
                {
                    if (i > 8 && sequence == 18)
                    {
                        i -= 9;
                    }

                    total += Convert.ToInt32(newCpf[i].ToString()) * reverse;

                    reverse -= 1;
                    if (reverse < 2)
                    {
                        reverse = 11;
                        double digito = 11 - (total % 11);

                        if (digito > 9)
                        {
                            digito = 0;
                        }

                        newCpf += Convert.ToString(digito);
                        total = 0;
                    }
                    sequence -= 1;
                }

                if (!(newCpf == Cpf))
                {
                    return false;
                }

                return true;
            }
            catch (FormatException e)
            {
                //capturando error de letras digitadas no campo cpf
                return false;
            }
        }

        private bool AvoidSequence(string cpf)
        {
            switch (cpf)
            {
                case "11111111111":
                    return true;
                    break;
                case "22222222222":
                    return true;
                    break;
                case "33333333333":
                    return true;
                    break;
                case "44444444444":
                    return true;
                    break;
                case "55555555555":
                    return true;
                    break;
                case "66666666666":
                    return true;
                    break;
                case "77777777777":
                    return true;
                    break;
                case "88888888888":
                    return true;
                    break;
                case "99999999999":
                    return true;
                    break;
                case "00000000000":
                    return true;
                    break;
            }
            return false;
        }
    }
}
