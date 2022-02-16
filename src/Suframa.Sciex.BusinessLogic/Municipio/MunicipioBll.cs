using Suframa.Sciex.CrossCutting.DataTransferObject;
using Suframa.Sciex.CrossCutting.DataTransferObject.ViewModel;
using Suframa.Sciex.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace Suframa.Sciex.BusinessLogic
{
    public class MunicipioBll : IMunicipioBll
    {
        private readonly IUnitOfWork _uow;

        public MunicipioBll(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<MunicipioDto> Listar(MunicipioDto municipioDto)
        {
            if (municipioDto == null) { municipioDto = new MunicipioDto(); }

            var lista = _uow.QueryStack.Municipio
                        .ListarGrafo<MunicipioDto>(s => new MunicipioDto
                        {
                            IdMunicipio = s.IdMunicipio,
                            Codigo = (int?)s.Codigo,
                            UF = s.SiglaUF,
                            SiglaUF = s.SiglaUF,
                            Descricao = s.Descricao
                        },
                x =>
                            (!municipioDto.IdMunicipio.HasValue || x.IdMunicipio == municipioDto.IdMunicipio)
                            && (!municipioDto.Codigo.HasValue || x.Codigo == municipioDto.Codigo)
                            && (string.IsNullOrEmpty(municipioDto.UF) || x.SiglaUF == municipioDto.UF)
                            && (string.IsNullOrEmpty(municipioDto.Descricao) || x.Descricao == municipioDto.Descricao)
                        )
                        .OrderBy(o => o.Descricao);

            return lista;
        }

	public IEnumerable<object> ListarChave(ViewMunicipioVM municipioVM)
	{

		if (municipioVM.Descricao == null && (municipioVM.IdMunicipio == null || municipioVM.IdMunicipio == -1))
		{
			return new List<object>();
		}

		var codigoConta = _uow.QueryStack.ViewMunicipio
			.Listar().Where(o =>
					(o.Descricao.ToLower().Contains(municipioVM.Descricao.ToLower()) ||
					(o.CodigoMunicipio != null && o.CodigoMunicipio.ToString().Contains(municipioVM.Descricao.ToString())) ||
					(municipioVM.IdMunicipio == null || o.CodigoMunicipio == municipioVM.IdMunicipio))
				)
			.OrderBy(o => o.Descricao)
			.ThenBy(x => x.Descricao)
			.Select(
				s => new
				{
					id = s.CodigoMunicipio,
					uf = s.SiglaUF,
					text = s.CodigoMunicipio + " | " + s.Descricao
				});


		return codigoConta;
	}

		public MunicipioDto Selecionar(MunicipioDto municipioDto)
        {
            var municipio = _uow.QueryStack.Municipio.Selecionar(x => x.IdMunicipio == municipioDto.IdMunicipio);
            return AutoMapper.Mapper.Map<MunicipioDto>(municipio);
        }
    }
}