using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectionHiddenCalculation.Commands
{
	public class CloseProjectCommand : ConnHiddenCalcCommandBase
	{
		public CloseProjectCommand(IConHiddenCalcModel model) : base(model)
		{
		}

		/// <summary>
		/// Is there a project to close ?
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		public override bool CanExecute(object parameter)
		{
			return Model.IsService;
		}

		/// <summary>
		/// Close the current idea connection project
		/// </summary>
		/// <param name="param"></param>
		public override void Execute(object parameter)
		{
			Model.CloseConnectionService();
		}
	}
}
