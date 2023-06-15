﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GymJournal.App.Services;
using GymJournal.App.Services.API;
using GymJournal.Domain.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymJournal.App.ViewModel.WorkoutPlanViewModels
{
	[QueryProperty("WorkoutPlanId","WorkoutPlanId")]
	public partial class WorkoutPlanUpsertPageViewModel : BaseViewModel
	{
		private readonly IWorkoutPlanService _workoutPlanService;
		private readonly IWorkoutService _workoutService;
		private readonly ExceptionHandlerService _exceptionHandlerService;

		public WorkoutPlanUpsertPageViewModel(IWorkoutPlanService workoutPlanService, IWorkoutService workoutService, ExceptionHandlerService exceptionHandlerService)
		{
			_workoutPlanService = workoutPlanService ?? throw new ArgumentNullException(nameof(workoutPlanService));
			_workoutService = workoutService ?? throw new ArgumentNullException(nameof(workoutService));
			_exceptionHandlerService = exceptionHandlerService ?? throw new ArgumentNullException(nameof(exceptionHandlerService));

			if (WorkoutPlanId == Guid.Empty)
				Title = "Add a new Exercise";
			else Title = "Update Exercise";
		}

		public Guid WorkoutPlanId { get; set; }
		[ObservableProperty]
		public WorkoutPlanDto upsertWorkoutPlan;

		public ObservableCollection<WorkoutDto> Workouts;

		public async Task OnAppearing()
		{
			if (IsBusy) return;

			try
			{
				IsBusy = true;

				Workouts = new ObservableCollection<WorkoutDto>(await _workoutService.GetAll());

				if(WorkoutPlanId != Guid.Empty) UpsertWorkoutPlan = await _workoutPlanService.GetById(WorkoutPlanId);
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
		public async Task SendForm()
		{
			if (IsBusy) return;

			try
			{
				IsBusy = true;

				if (WorkoutPlanId == Guid.Empty)
					UpsertWorkoutPlan = await _workoutPlanService.Add(UpsertWorkoutPlan);
				else
					UpsertWorkoutPlan = await _workoutPlanService.Update(UpsertWorkoutPlan);

				await Shell.Current.Navigation.PopAsync();
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
	}
}
