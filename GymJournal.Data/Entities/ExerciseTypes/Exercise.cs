﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymJournal.Data.Entities.ExerciseTypes
{
    public class Exercise
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? YouTubeLink { get; set; }
        public int Likes { get; set; }
        public bool IsBegginerFriendly { get; set; }
        public bool IsGymRequired { get; set; }
        public bool IsPrivate { get; set; }
		public ICollection<Muscle> Muscles { get; set; }
        public ICollection<ExerciseEntry> ExerciseEntries { get; set; }
        public ICollection<PastExercise> PastExercises { get; set; }
	}
}