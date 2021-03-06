﻿using System;
using System.Diagnostics;
using Xamarin.Forms;
using XampleUI.Models;

namespace XampleUI.ViewModels.Groc
{
	[QueryProperty(nameof(ItemId), nameof(ItemId))]
	internal class GrocDetailViewModel : BaseViewModel
	{
		private Item item;
		private string itemId;
		private int quantity;
		public Command AddItemCommand => new Command(OnAddItem);
		public Command CountAddCommand => new Command(CountAdd);
		public Command CountMinusCommand => new Command(CountMinus);

		public Item CurrentItem
		{
			get => item;
			set => SetProperty(ref item, value);
		}

		public string ItemId
		{
			get => itemId;
			set
			{
				itemId = value;
				LoadItemId(value);
			}
		}

		public int Quantity
		{
			get => quantity;
			set => SetProperty(ref quantity, value);
		}

		public INavigation Navigation { get; set; }

		public async void LoadItemId(string itemId)
		{
			try
			{
				CurrentItem = await DataStore.GetGrocAsync(itemId);
			}
			catch (Exception)
			{
				Debug.WriteLine("Failed to Load Item");
			}
		}

		private void CountAdd()
		{
			Quantity += 1;
		}

		private void CountMinus()
		{
			if (Quantity > 0)
			{
				Quantity -= 1;
			}
		}

		private async void OnAddItem(object obj)
		{
			if (Quantity < 1)
			{
				await Application.Current.MainPage.DisplayAlert("Error", "Qty is 0.", "Ok");
				return;
			}

			var cartItem = new ItemCart(CurrentItem)
			{
				Quantity = Quantity
			};
			await DataStore.AddItemToCartAsync(cartItem);

			await Navigation.PopAsync();
		}
	}
}