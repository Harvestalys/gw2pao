﻿using GW2PAO.API.Data;
using GW2PAO.API.Data.Entities;
using System;

namespace GW2PAO.API.Services.Interfaces
{
	public interface ICyclesService
	{
		/// <summary>
		/// The World Events time table
		/// </summary>
		CycleTimeTable TimeTable { get; }

		/// <summary>
		/// Loads the events time table and initializes all cached event information
		/// </summary>
		void LoadTable();

		/// <summary>
		/// Returns the localized name for the given event
		/// </summary>
		/// <param name="id">ID of the event to return the name of</param>
		/// <returns>The localized name</returns>
		//string GetLocalizedName(Guid id);

		/// <summary>
		/// Retrieves the current state of the given event
		/// </summary>
		/// <param name="id">The ID of the event to retrieve the state of</param>
		/// <returns>The current state of the input event</returns>
		Data.Enums.EventState GetState(Guid id);

		/// <summary>
		/// Retrieves the current state of the given event
		/// </summary>
		/// <param name="evt">The event to retrieve the state of</param>
		/// <returns>The current state of the input event</returns>
		Data.Enums.EventState GetState(Cycle c);

		/// <summary>
		/// Retrieves the amount of time until the next active time for the given event, using the megaserver timetables
		/// </summary>
		/// <param name="evt">The event to retrieve the time for</param>
		/// <returns>Timespan containing the amount of time until the event is next active</returns>
		TimeSpan GetTimeUntilActive(Cycle c);

		/// <summary>
		/// Retrieves the amount of time since the last active time for the given event, using the megaserver timetables
		/// </summary>
		/// <param name="evt">The event to retrieve the time for</param>
		/// <returns>Timespan containing the amount of time since the event was last active</returns>
		TimeSpan GetTimeSinceActive(Cycle c);
	}
}