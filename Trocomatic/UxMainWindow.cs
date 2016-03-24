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

namespace Trocomatic
{
	public partial class UxMainForm : Form
	{
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
			long paidAmount = long.Parse(this.UxTbxPaidValue.Text);
			long productAmount = long.Parse(this.UxTbxProductValue.Text);
			//Pegar lista de moedas
			GetChangeResponse response = this.CalculateChange(paidAmount, productAmount);
			this.CleanForm();
			if (response.Success && response.Coins != null && response.Coins.Any())
			{ 
				//Exibir no textbox
				
				this.UxTbxChangeResult.Text += "Total de troco: " + response.TotalChangeAmount + Environment.NewLine;
				var groups = response.Coins.GroupBy(c => c.Amount);
				foreach (var g in groups)
				{
					this.UxTbxChangeResult.Text += g.Count() + " Moedas de " + g.Key.ToString() + Environment.NewLine;
				}
			}
			else
			{
				foreach(var r in response.Reports){
					this.UxTbxChangeResult.Text +=  r.Message + Environment.NewLine;
				}
				
			}


		}

		private GetChangeResponse CalculateChange(long paidValue, long productValue)
		{
			TrocomaticManager manager = new TrocomaticManager();
			GetChangeRequest request = new GetChangeRequest();

			request.PaidAmount = paidValue;
			request.ProductAmount = productValue;

			GetChangeResponse response = manager.GetChange(request);

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
