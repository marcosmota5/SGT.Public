using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls.Dialogs;
using Model.DataAccessLayer.HelperClasses;
using SGT.HelperClasses;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace SGT.ViewModels
{
    public class ParametrosViewModel : ObservableObject, IPageViewModel
    {
        #region Campos

        private bool _selecaoParametrosHabilitado = true;
        private IPageViewModel _currentPageViewModel;
        private ObservableCollection<PaginaParametro> _listaPaginasParametros = new();
        private PaginaParametro _paginaParametroSelecionado;

        #endregion Campos

        #region Construtores

        public ParametrosViewModel()
        {
            Messenger.Default.Register<bool>(this, "SelecaoParametrosHabilitado", delegate (bool selecaoParametrosHabilitado) { SelecaoParametrosHabilitado = selecaoParametrosHabilitado; });

            Grupo grupoGeral = new() { Nome = "Geral", Icone = "Ballot", Visivel = true };
            Grupo grupoEquipamentos = new() { Nome = "Equipamentos", Icone = "CarMultiple", Visivel = true };
            Grupo grupoPropostas = new() { Nome = "Propostas", Icone = "Handshake", Visivel = true };
            Grupo grupoSistema = new() { Nome = "Sistema", Icone = "Application", Visivel = App.Usuario.Id == 1 };
            Grupo grupoOrdensServico = new() { Nome = "Ordens de serviço", Icone = "CarWrench", Visivel = true };

            // Grupo geral
            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Clientes",
                Icone = "AccountTie",
                Grupo = grupoGeral,
                Visivel = true,
                Pagina = new ParametroClientesViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Plantas",
                Icone = "Nature",
                Grupo = grupoGeral,
                Visivel = true,
                Pagina = new ParametroPlantasViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Áreas",
                Icone = "NaturePeople",
                Grupo = grupoGeral,
                Visivel = true,
                Pagina = new ParametroAreasViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Frotas",
                Icone = "CarMultiple",
                Grupo = grupoGeral,
                Visivel = true,
                Pagina = new ParametroFrotasViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Contatos",
                Icone = "CardAccountPhone",
                Grupo = grupoGeral,
                Visivel = true,
                Pagina = new ParametroContatosViewModel(DialogCoordinator.Instance)
            });

            // Grupo equipamentos
            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Fabricantes",
                Icone = "RobotIndustrial",
                Grupo = grupoEquipamentos,
                Visivel = true,
                Pagina = new ParametroFabricantesViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Tipos de equipamentos",
                Icone = "FormatListBulletedType",
                Grupo = grupoEquipamentos,
                Visivel = true,
                Pagina = new ParametroTiposEquipamentosViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Categorias",
                Icone = "Shape",
                Grupo = grupoEquipamentos,
                Visivel = true,
                Pagina = new ParametroCategoriasViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Classes",
                Icone = "ShapePlus",
                Grupo = grupoEquipamentos,
                Visivel = true,
                Pagina = new ParametroClassesViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Modelos",
                Icone = "Forklift",
                Grupo = grupoEquipamentos,
                Visivel = true,
                Pagina = new ParametroModelosViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Famílias",
                Icone = "FamilyTree",
                Grupo = grupoEquipamentos,
                Visivel = true,
                Pagina = new ParametroFamiliasViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Anos",
                Icone = "CalendarMonth",
                Grupo = grupoEquipamentos,
                Visivel = true,
                Pagina = new ParametroAnosViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Séries",
                Icone = "FormatListNumbered",
                Grupo = grupoEquipamentos,
                Visivel = true,
                Pagina = new ParametroSeriesViewModel(DialogCoordinator.Instance)
            });

            // Grupo propostas
            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Status da aprovação",
                Icone = "ListStatus",
                Grupo = grupoPropostas,
                Visivel = true,
                Pagina = new ParametroStatusAprovacaoViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Justificativas da aprovação",
                Icone = "FormatAlignJustify",
                Grupo = grupoPropostas,
                Visivel = true,
                Pagina = new ParametroJustificativasAprovacaoViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Fornecedores",
                Icone = "CarLiftedPickup",
                Grupo = grupoPropostas,
                Visivel = true,
                Pagina = new ParametroFornecedoresViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Termos",
                Icone = "FileDocumentMultiple",
                Grupo = grupoPropostas,
                Visivel = true,
                Pagina = new ParametroTermosViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Conjuntos",
                Icone = "TableColumn",
                Grupo = grupoPropostas,
                Visivel = true,
                Pagina = new ParametroConjuntosViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Especificações",
                Icone = "Table",
                Grupo = grupoPropostas,
                Visivel = true,
                Pagina = new ParametroEspecificacoesViewModel(DialogCoordinator.Instance)
            });

            // Grupo ordens de serviço
            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Tipos de ordem de serviço",
                Icone = "CardBulletedOutline",
                Grupo = grupoOrdensServico,
                Visivel = true,
                Pagina = new ParametroTiposOrdemServicoViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Executantes do serviço",
                Icone = "HardHat",
                Grupo = grupoOrdensServico,
                Visivel = true,
                Pagina = new ParametroExecutantesServicoViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Usos indevidos",
                Icone = "HeadAlert",
                Grupo = grupoOrdensServico,
                Visivel = true,
                Pagina = new ParametroUsosIndevidosViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Eventos",
                Icone = "PlaylistStar",
                Grupo = grupoOrdensServico,
                Visivel = true,
                Pagina = new ParametroEventosViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Inconsistências",
                Icone = "MessageAlert",
                Grupo = grupoOrdensServico,
                Visivel = true,
                Pagina = new ParametroInconsistenciasViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Status do equipamento após a manutenção",
                Icone = "ListStatus",
                Grupo = grupoOrdensServico,
                Visivel = true,
                Pagina = new ParametroStatusEquipamentoAposManutencaoViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Tipos de manutenção",
                Icone = "PipeWrench",
                Grupo = grupoOrdensServico,
                Visivel = true,
                Pagina = new ParametroTiposManutencaoViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Motivos",
                Icone = "HelpCircle",
                Grupo = grupoOrdensServico,
                Visivel = true,
                Pagina = new ParametroMotivosViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Fornecimentos",
                Icone = "Dolly",
                Grupo = grupoOrdensServico,
                Visivel = true,
                Pagina = new ParametroFornecimentosViewModel(DialogCoordinator.Instance)
            });

            // Grupo sistema
            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Configurações do sistema",
                Icone = "CogOutline",
                Grupo = grupoSistema,
                Visivel = true,
                Pagina = new ParametroConfiguracoesSistemaViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Empresas",
                Icone = "OfficeBuilding",
                Grupo = grupoSistema,
                Visivel = true,
                Pagina = new ParametroEmpresasViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Filiais",
                Icone = "OfficeBuildingMarker",
                Grupo = grupoSistema,
                Visivel = true,
                Pagina = new ParametroFiliaisViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Setores",
                Icone = "ChairRolling",
                Grupo = grupoSistema,
                Visivel = true,
                Pagina = new ParametroSetoresViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Taxas",
                Icone = "SackPercent",
                Grupo = grupoSistema,
                Visivel = true,
                Pagina = new ParametroTaxasViewModel(DialogCoordinator.Instance)
            });

            ListaPaginasParametros.Add(new PaginaParametro
            {
                Nome = "Colunas para importação",
                Icone = "FormatColumns",
                Grupo = grupoSistema,
                Visivel = true,
                Pagina = new ParametroColunasImportacaoViewModel(DialogCoordinator.Instance)
            });
        }

        #endregion Construtores

        #region Propriedades / Comandos

        public string Name
        {
            get
            {
                return "Parâmetros";
            }
        }

        public string Icone
        {
            get
            {
                return "";
            }
        }

        public void LimparViewModel()
        {
            try
            {
                try
                {
                    foreach (var item in _listaPaginasParametros)
                    {
                        item.Pagina.LimparViewModel();
                    }
                }
                catch (Exception)
                {
                }
                _currentPageViewModel = null;
                _listaPaginasParametros = null;
                _paginaParametroSelecionado = null;
            }
            catch (Exception)
            {
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                if (_currentPageViewModel != value)
                {
                    _currentPageViewModel = value;
                    OnPropertyChanged(nameof(CurrentPageViewModel));
                }
            }
        }

        public bool SelecaoParametrosHabilitado
        {
            get
            {
                return _selecaoParametrosHabilitado;
            }
            set
            {
                if (_selecaoParametrosHabilitado != value)
                {
                    _selecaoParametrosHabilitado = value;
                    OnPropertyChanged(nameof(SelecaoParametrosHabilitado));
                }
            }
        }

        public ObservableCollection<PaginaParametro> ListaPaginasParametros
        {
            get
            {
                return _listaPaginasParametros;
            }
            set
            {
                if (_listaPaginasParametros != value)
                {
                    _listaPaginasParametros = value;
                    OnPropertyChanged(nameof(ListaPaginasParametros));
                }
            }
        }

        public PaginaParametro PaginaParametroSelecionado
        {
            get
            {
                return _paginaParametroSelecionado;
            }
            set
            {
                if (_paginaParametroSelecionado != value)
                {
                    _paginaParametroSelecionado = value;

                    CurrentPageViewModel = PaginaParametroSelecionado.Pagina;
                    try
                    {
                        ((dynamic)PaginaParametroSelecionado.Pagina).ConstrutorAsync();
                    }
                    catch (Exception)
                    {
                    }
                    OnPropertyChanged(nameof(PaginaParametroSelecionado));
                }
            }
        }

        public ICommand ComandoFechar { get; }

        #endregion Propriedades / Comandos
    }

    public class PaginaParametro
    {
        public string Nome { get; set; }
        public string Icone { get; set; }
        public Grupo Grupo { get; set; }
        public bool Visivel { get; set; }
        public IPageViewModel Pagina { get; set; }
    }

    public class Grupo
    {
        public string Nome { get; set; }
        public string Icone { get; set; }
        public bool Visivel { get; set; }
    }
}