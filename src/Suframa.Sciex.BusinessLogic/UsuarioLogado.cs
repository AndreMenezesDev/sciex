using FluentValidation;
using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.Enum;
using Suframa.Sciex.CrossCutting.Security;
using Suframa.Sciex.DataAccess;
using System;

namespace Suframa.Sciex.BusinessLogic
{
	public class UsuarioLogado : IUsuarioLogado
	{
		private readonly IUsuarioInformacoesBll _usuarioInformacoesBll;

		/// <summary>
		/// Objeto do usuario logado
		/// </summary>
		public TokenDto Usuario { get; set; }

		/// <summary>
		/// Construtor
		/// </summary>
		/// <param name="uow"></param>
		/*public UsuarioLogado(IUsuarioInformacoesBll usuarioInformacoesBll)
		{
			this._usuarioInformacoesBll = usuarioInformacoesBll;
			this.Usuario = new TokenDto();
			var token = JwtManager.GetTokenFromHeader();
			if (string.IsNullOrEmpty(token))
				return;

			this.Usuario = JwtManager.GetPrincipal(token);
			if (this.Usuario == null)
			{
				throw new ValidationException("Usuario não reconhecido");
			}
		}*/

		public void Load()
		{
			var cpfCnpj = new CrossCutting.SuperStructs.CpfCnpj(this.Usuario.CpfCnpj);
			if (cpfCnpj.DocumentType == CrossCutting.SuperStructs.CpfCnpj.DocumentTypes.Cpf)
			{
				this.Usuario.TipoPessoa = EnumTipoPessoa.PessoaFisica;
				this.Usuario.Cpf = cpfCnpj.Unmasked;

				var usuarioInterno = this._usuarioInformacoesBll.ObterUsuarioInterno(this.Usuario.Cpf);
				this.Usuario.IdUsuarioInterno = usuarioInterno?.IdUsuarioInterno;
				this.Usuario.IdPessoaFisica = this._usuarioInformacoesBll.ObterIdPessoaFisica(this.Usuario.Cpf);

				if (this.Usuario.IdUsuarioInterno.HasValue)
				{
					this.Usuario.Papeis = this._usuarioInformacoesBll.ListarPapeis(this.Usuario.IdUsuarioInterno.Value);
					this.Usuario.Setor = this._usuarioInformacoesBll.ObterSetorUsuarioInterno(this.Usuario.Cpf);
					this.Usuario.NomeUsuario = usuarioInterno?.Nome;
				}

				if (this.Usuario.IdPessoaFisica.HasValue)
					this.Usuario.Perfis = this._usuarioInformacoesBll.ListarPerfis(this.Usuario.IdPessoaFisica, null);
			}
			else if (cpfCnpj.DocumentType == CrossCutting.SuperStructs.CpfCnpj.DocumentTypes.Cnpj)
			{
				this.Usuario.TipoPessoa = EnumTipoPessoa.PessoaJuridica;
				this.Usuario.Cnpj = cpfCnpj.Unmasked;
				this.Usuario.IdPessoaJuridica = this._usuarioInformacoesBll.ObterIdPessoaJuridica(this.Usuario.Cnpj);

				if (this.Usuario.IdPessoaJuridica.HasValue)
					this.Usuario.Perfis = this._usuarioInformacoesBll.ListarPerfis(null, this.Usuario.IdPessoaJuridica);
			}
			else
			{
				throw new ValidationException("Documento do usuário não reconhecido");
			}

			this.Usuario.UsuarioInterno = true;
		}
	}
}