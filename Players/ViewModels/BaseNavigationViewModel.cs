using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MvvmHelpers;
using Xamarin.Forms;

namespace Players.ViewModels
{
    public class BaseNavigationViewModel : BaseViewModel, INavigation
	{
		Page _CurrentPage => Application.Current?.MainPage.Navigation.NavigationStack[0];
		INavigation _Navigation => Application.Current?.MainPage?.Navigation;
		Application _App => Application.Current;

		#region INavigation implementation

		public async Task SetRootPageAsync(Page page)
		{
			await PopToRootAsync();
			_App.MainPage = page;
		}

		public Page GetMainPage()
		{
			return _CurrentPage;
		}

		public void RemovePage(Page page)
		{
			_Navigation?.RemovePage(page);
		}

		public void InsertPageBefore(Page page, Page before)
		{
			_Navigation?.InsertPageBefore(page, before);
		}

		public async Task PushAsync(Page page)
		{
			var task = _Navigation?.PushAsync(page);
			if (task != null)
				await task;
		}

		public async Task<Page> PopAsync()
		{
			var task = _Navigation?.PopAsync();
			return task != null ? await task : await Task.FromResult(null as Page);
		}

		public async Task PopToRootAsync()
		{
			var task = _Navigation?.PopToRootAsync();
			if (task != null)
				await task;
		}

		public async Task PushModalAsync(Page page)
		{
			var task = _Navigation?.PushModalAsync(page);
			if (task != null)
				await task;
		}

		public async Task<Page> PopModalAsync()
		{
			var task = _Navigation?.PopModalAsync();
			return task != null ? await task : await Task.FromResult(null as Page);
		}

		public async Task PushAsync(Page page, bool animated)
		{
			var task = _Navigation?.PushAsync(page, animated);
			if (task != null)
				await task;
		}

		public async Task<Page> PopAsync(bool animated)
		{
			var task = _Navigation?.PopAsync(animated);
			return task != null ? await task : await Task.FromResult(null as Page);
		}

		public async Task PopToRootAsync(bool animated)
		{
			var task = _Navigation?.PopToRootAsync(animated);
			if (task != null)
				await task;
		}

		public async Task PushModalAsync(Page page, bool animated)
		{
			var task = _Navigation?.PushModalAsync(page, animated);
			if (task != null)
				await task;
		}

		public async Task<Page> PopModalAsync(bool animated)
		{
			var task = _Navigation?.PopModalAsync(animated);
			return task != null ? await task : await Task.FromResult(null as Page);
		}

		public IReadOnlyList<Page> NavigationStack => _Navigation?.NavigationStack;
		public IReadOnlyList<Page> ModalStack => _Navigation?.ModalStack;

		#endregion INavigation implementation
	}

}
