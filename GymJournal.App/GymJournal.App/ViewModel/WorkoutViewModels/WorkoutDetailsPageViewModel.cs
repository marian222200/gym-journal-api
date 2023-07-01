﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymJournal.App.Services;
using GymJournal.App.Services.API;
using GymJournal.App.View.ExercisePages;
using GymJournal.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymJournal.App.ViewModel.WorkoutViewModels
{
	[QueryProperty("WorkoutId", "WorkoutId")]
	public partial class WorkoutDetailsPageViewModel : BaseViewModel
	{
		private readonly IWorkoutService _workoutService;
		private readonly ExceptionHandlerService _exceptionHandlerService;

		public WorkoutDetailsPageViewModel(IWorkoutService workoutService, ExceptionHandlerService exceptionHandlerService)
		{
			_workoutService = workoutService ?? throw new ArgumentNullException(nameof(workoutService));
			_exceptionHandlerService = exceptionHandlerService ?? throw new ArgumentNullException(nameof(exceptionHandlerService));

			Title = "Workout Details";
		}

		public Guid WorkoutId { get; set; }

		[ObservableProperty]
		public WorkoutDto detailsWorkout;

		public async Task OnAppearing()
		{
			if (IsBusy) return;

			try
			{
				IsBusy = true;

				DetailsWorkout = await _workoutService.GetById(WorkoutId);

				Title = DetailsWorkout.Name;
			}
			catch (Exception ex)
			{
				await _exceptionHandlerService.HandleException(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}

		[RelayCommand]
		public async void Delete()
		{
			if (IsBusy) return;

			try
			{
				IsBusy = true;

				await _workoutService.Delete(DetailsWorkout.Id);
			}
			catch (Exception ex)
			{
				await _exceptionHandlerService.HandleException(ex);
			}
			finally
			{
				IsBusy = false;
			}
		}



		[RelayCommand]
		public async Task GoToDetailsAsync(ExerciseDto exercise)
		{
			if (exercise is null)
				return;

			await Shell.Current.GoToAsync($"{nameof(ExerciseDetailsPage)}", true,
				new Dictionary<string, object>
				{
					{"ExerciseId", exercise.Id}
				});
		}
	}
}