using System;

namespace Suframa.Sciex.CrossCutting.SuperStructs
{
    public struct CpfCnpj
    {
        private object cpfCnpj;
        private string value;
        public DocumentTypes? DocumentType { get; private set; }
        public bool IsValid { get; private set; }
        public string Masked { get; private set; }
        public Type ObjectType { get; private set; }
        public string Unmasked { get; private set; }

        public enum DocumentTypes
        {
            Cpf,
            Cnpj
        }

        public CpfCnpj(string value)
        {
            value = value ?? string.Empty;
            this.value = value.ExtractNumbers();

            if (this.value.Length == 11)
            {
                Cpf cpf = new Cpf(this.value);
                this.DocumentType = DocumentTypes.Cpf;
                this.Unmasked = cpf.Unmasked;
                this.Masked = cpf.Masked;
                this.IsValid = cpf.IsValid;
                this.cpfCnpj = cpf;
                this.ObjectType = cpf.GetType();
            }
            else if (this.value.Length == 14)
            {
                Cnpj cnpj = new Cnpj(this.value);
                this.DocumentType = DocumentTypes.Cnpj;
                this.Unmasked = cnpj.Unmasked;
                this.Masked = cnpj.Masked;
                this.IsValid = cnpj.IsValid;
                this.cpfCnpj = cnpj;
                this.ObjectType = cnpj.GetType();
            }
            else
            {
                this.DocumentType = null;
                this.Unmasked = null;
                this.Masked = null;
                this.IsValid = false;
                this.cpfCnpj = null;
                this.ObjectType = null;
            }
        }

        public static implicit operator Cnpj(CpfCnpj d)
        {
            return new Cnpj(d.ToString());
        }

        public static implicit operator Cpf(CpfCnpj d)
        {
            return new Cpf(d.ToString());
        }

        public static implicit operator CpfCnpj(string d)
        {
            return new CpfCnpj(d);
        }

        public static implicit operator string(CpfCnpj d)
        {
            return d.ToString();
        }

        public static string Mask(string value)
        {
            Cpf cpf = new Cpf(value);
            Cnpj cnpj = new Cnpj(value);

            if (cpf.IsValid)
            {
                return cpf.Masked;
            }
            else if (cnpj.IsValid)
            {
                return cnpj.Masked;
            }

            return null;
        }

        public static string Unmask(string value)
        {
            Cpf cpf = new Cpf(value);
            Cnpj cnpj = new Cnpj(value);

            if (cpf.IsValid)
            {
                return cpf.Unmasked;
            }
            else if (cnpj.IsValid)
            {
                return cnpj.Unmasked;
            }

            return null;
        }

        public static bool Validate(string value)
        {
            Cpf cpf = new Cpf(value);
            Cnpj cnpj = new Cnpj(value);

            if (cpf.IsValid)
            {
                return true;
            }
            else if (cnpj.IsValid)
            {
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return this.Unmasked;
        }
    }
}