﻿using GymJournal.API.Models;
using GymJournal.Data.Entities;
using GymJournal.Data.Repositories;
using GymJournal.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GymJournal.API.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WorkoutController : ControllerBase
	{
		private readonly IRepository<WorkoutDto> _workoutRepository;

		public WorkoutController(IRepository<WorkoutDto> workoutRepository)
		{
			_workoutRepository = workoutRepository ?? throw new ArgumentNullException(nameof(workoutRepository));
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			try
			{
				var workouts = await _workoutRepository.GetAll();

				return Ok(workouts);
			}
			catch (Exception ex)
			{
				var errorResponse = new ErrorResponse
				{
					Message = ex.Message,
				};

				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(Guid? id)
		{
			try
			{
				if (id == null)
				{
					return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse { Message = "Trying to GetById Workout with null Id is invalid." });
				}

				var workout = await _workoutRepository.GetById(id);

				return Ok(workout);
			}
			catch (Exception ex)
			{
				var errorResponse = new ErrorResponse
				{
					Message = ex.Message,
				};

				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(Guid? id)
		{
			try
			{
				var workout = await _workoutRepository.GetById(id);

				if (workout == null)
				{
					return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse { Message = "Trying to Delete Workout with invalid Id." });
				}

				await _workoutRepository.Remove(id);

				return StatusCode(StatusCodes.Status204NoContent);
			}
			catch (Exception ex)
			{
				var errorResponse = new ErrorResponse
				{
					Message = ex.Message,
				};

				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] WorkoutDto workout)
		{
			try
			{
				if (workout == null)
				{
					return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse { Message = "Trying to Add null Workout." });
				}

				if (workout.ExerciseIds == null) workout.ExerciseIds = new List<Guid>();
				if (workout.WorkoutPlanIds == null) workout.WorkoutPlanIds = new List<Guid>();

				var responseWorkout = await _workoutRepository.Add(workout);

				return Created(string.Empty, responseWorkout);
			}
			catch (Exception ex)
			{
				var errorResponse = new ErrorResponse
				{
					Message = ex.Message,
				};

				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}

		[HttpPut]
		public async Task<IActionResult> Update([FromBody] WorkoutDto workout)
		{
			try
			{
				var existentWorkout = await _workoutRepository.GetById(workout.Id);

				if (existentWorkout == null)
				{
					return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse { Message = "Trying to Update Workout with not existent Id." });
				}

				var responseWorkout = await _workoutRepository.Update(workout);

				return Ok(responseWorkout);
			}
			catch (Exception ex)
			{
				var errorResponse = new ErrorResponse
				{
					Message = ex.Message,
				};

				return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
			}
		}
	}
}