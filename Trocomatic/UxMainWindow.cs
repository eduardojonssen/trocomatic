﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trocomatic.Core;
using Trocomatic.Core.DataContracts;
using Trocomatic.Infrastructure;

namespace Trocomatic
{
	public partial class UxMainForm : Form
	{
		 
			 
		private TrocomaticManager _manager = new 
			TrocomaticManager(new ChangeProcessorFactory());
			
		public UxMainForm()
		{
			InitializeComponent();
		}

		private void UxBtnOk_Click(object sender, EventArgs e)
		{
			this.DisplayChange();
		}

		private void DisplayChange()
		{
			try
			{
				long paidAmount = long.Parse(this.UxTbxPaidValue.Text);
				long productAmount = long.Parse(this.UxTbxProductValue.Text);

				//Pegar lista de moedas
				GetChangeResponse response = this.CalculateChange(paidAmount, productAmount);

				this.CleanForm();
				if (response.Success && response.Details != null && response.Details.Any())
				{
					//Exibir no textbox
					this.UxTbxChangeResult.Text += "Total de troco: " + response.TotalChangeAmount + Environment.NewLine;

					foreach (var d in response.Details)
					{
						this.UxTbxChangeResult.Text += d.Quantity + " " + d.MoneyType + " de " + d.Amount + Environment.NewLine;
					}
				}
				else
				{
					foreach (var r in response.Reports)
					{
						this.UxTbxChangeResult.Text += r.Message + Environment.NewLine;
					}
				}
			}
			catch (Exception ex)
			{
				LoggerFactory.GetLogger().Save(ex, LogType.Exception);
				MessageBox.Show("Erro inesperado.");
			}
			
		}

		private GetChangeResponse CalculateChange(long paidValue, long productValue)
		{
			GetChangeRequest request = new GetChangeRequest();

			request.PaidAmount = paidValue;
			request.ProductAmount = productValue;

			GetChangeResponse response = this._manager.GetChange(request);

			return response;
		}

		private void UxBtnCancel_Click(object sender, EventArgs e)
		{
			this.CleanForm();
		}

		private void CleanForm()
		{
			this.UxTbxPaidValue.Text = string.Empty;
			this.UxTbxProductValue.Text = string.Empty;
			this.UxTbxChangeResult.Text = string.Empty;
		}
	}
}