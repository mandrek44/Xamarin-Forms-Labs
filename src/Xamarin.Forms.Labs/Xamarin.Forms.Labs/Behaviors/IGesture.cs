﻿namespace Xamarin.Forms.Labs.Behaviors
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface implmenented to consume gestures
    /// analagous to ICommand
    /// </summary>
    public interface IGesture
    {
        /// <summary>
        /// Execute the gesture
        /// </summary>
        /// <param name="result">The <see cref="GestureResult"/></param>
        /// <param name="param">the user supplied paramater</param>
        void ExecuteGesture(GestureResult result, object param);
        /// <summary>
        /// Checks to see if the gesture should execute
        /// </summary>
        /// <param name="result">The <see cref="GestureResult"/></param>
        /// <param name="param">The user supplied parameter</param>
        /// <returns>True to execute the gesture, False otherwise</returns>
        bool CanExecuteGesture(GestureResult result, object param);
    }

    /// <summary>
    /// Syncronous Implmentation of the IGesture
    /// Paramater is a objet type
    /// </summary>
    public class RelayGesture : IGesture
    {
        private readonly Action<GestureResult, object> execute = null;
        private readonly Func<GestureResult,object,bool> canexecute = null;

        /// <summary>
        /// Builds the Rely Gesture
        /// </summary>
        /// <param name="execute">The action to execute when the gesture occures</param>
        /// <param name="predicate">A function to determine if the action should fire. If ommited the action is always available.</param>
        public RelayGesture(Action<GestureResult,object>execute,Func<GestureResult,object,bool>predicate=null)
        {
            this.execute = execute;
            canexecute = predicate;
        }

        /// <summary>
        /// Excutes the action assoicated with the gesture
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="annoyingbaseobjectthing">The Gesture Paramater</param>
        public void ExecuteGesture(GestureResult result, object annoyingbaseobjectthing)
        {
            if (execute != null) execute(result, annoyingbaseobjectthing);
        }

        /// <summary>
        /// Tests to see if a gesture's action can execute
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="annoyingbaseobjectthing">The Gesture Paramater</param>
        /// <returns>true if the action can execute,false othewise</returns>
        public bool CanExecuteGesture(GestureResult result, object annoyingbaseobjectthing)
        {
            return (canexecute == null || canexecute(result, annoyingbaseobjectthing));
        }       
    }

    /// <summary>
    /// Syncronous Implmentation of the IGesture
    /// Paramater is a T type
    /// </summary>
    public class RelayGesture<T> : IGesture where T:class
    {
        private readonly Action<GestureResult, T> execute = null;
        private readonly Func<GestureResult, T, bool> canexecute = null;


        /// <summary>
        /// Builds the Rely Gesture
        /// </summary>
        /// <param name="execute">The action to execute when the gesture occures</param>
        /// <param name="predicate">A function to determine if the action should fire. If ommited the action is always available.</param>
        public RelayGesture(Action<GestureResult, T> execute, Func<GestureResult, T, bool> predicate = null)
        {
            this.execute = execute;
            canexecute = predicate;
        }

        /// <summary>
        /// Excutes the action assoicated with the gesture
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="param">The Gesture Paramater cast to T before calling the action</param>
        public void ExecuteGesture(GestureResult result, object param)
        {
            if (execute != null) execute(result, param as T);
        }

        /// <summary>
        /// Tests to see if a gesture's action can execute
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="param">The Gesture Paramater cast to T before calling the function</param>
        /// <returns>true if the action can execute,false othewise</returns>
        public bool CanExecuteGesture(GestureResult result, object param)
        {
            return (canexecute == null || canexecute(result, param as T));
        }
    }

    /// <summary>
    /// ASyncronous Implmenta tion of the IGesture
    /// The execute is asyncronous while the canexecute is syncronous
    /// Paramater is an object type
    /// </summary>
    public class RelayGestureAsync : IGesture
    {
        private readonly Func<GestureResult, object, Task> asyncExecute;
        private readonly Func<GestureResult, object, bool> canexecute;

        /// <summary>
        /// Builds the Rely Gesture
        /// </summary>
        /// <param name="execute">The asyncronous action to execute when the gesture occures</param>
        /// <param name="predicate">A function to determine if the action should fire. If ommited the action is always available.</param>
        public RelayGestureAsync(Func<GestureResult, object, Task> execute, Func<GestureResult, object, bool> predicate)
        {
            asyncExecute = execute;
            canexecute = predicate;
        }
        /// <summary>
        /// Excutes the asyncronous action assoicated with the gesture
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="param">The Gesture Paramater</param>
        public async void ExecuteGesture(GestureResult result, object param)
        {
            if (asyncExecute != null) await Execute(result, param);
        }

        /// <summary>
        /// Tests to see if a gesture's action can execute
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="annoyingbaseobjectthing">The Gesture Paramater</param>
        /// <returns>true if the action can execute,false othewise</returns>
        public bool CanExecuteGesture(GestureResult result, object annoyingbaseobjectthing)
        {
            return (canexecute == null || canexecute(result,annoyingbaseobjectthing));
        }
        /// <summary>
        /// Virtual aync funciton that the user can override to provide
        /// any custom functionality required.
        /// </summary>
        /// <param name="gesture"><see cref="GestureResult"/></param>
        /// <param name="annoyingbaseobjectthing"></param>
        /// <returns></returns>
        protected virtual async Task Execute(GestureResult gesture, object annoyingbaseobjectthing)
        {
            await asyncExecute(gesture, annoyingbaseobjectthing);
        }
    }

    /// <summary>
    /// ASyncronous Implmenta tion of the IGesture
    /// The execute is asyncronous while the canexecute is syncronous
    /// Paramater is an T type
    /// </summary>
    public class RelayGestureAsync<T> : IGesture where T:class
    {
        private readonly Func<GestureResult, T, Task> asyncExecute;
        private readonly Func<GestureResult, T, bool> canexecute;

        /// <summary>
        /// Builds the Rely Gesture
        /// </summary>
        /// <param name="execute">The asyncronous action to execute when the gesture occures</param>
        /// <param name="predicate">A function to determine if the action should fire. If ommited the action is always available.</param>
        public RelayGestureAsync(Func<GestureResult, T, Task> execute, Func<GestureResult, T, bool> predicate)
        {
            asyncExecute = execute;
            canexecute = predicate;
        }
        /// <summary>
        /// Excutes the asyncronous action assoicated with the gesture
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="param">The Gesture Paramater</param>
        public async void ExecuteGesture(GestureResult result, object param)
        {
            if (asyncExecute != null) await Execute(result, param);
        }

        /// <summary>
        /// Tests to see if a gesture's action can execute
        /// </summary>
        /// <param name="result">The final <see cref="GestureResult"/></param>
        /// <param name="annoyingbaseobjectthing">The Gesture Paramater</param>
        /// <returns>true if the action can execute,false othewise</returns>
        public bool CanExecuteGesture(GestureResult result, object annoyingbaseobjectthing)
        {
            return (canexecute == null || canexecute(result, annoyingbaseobjectthing as T));
        }
        /// <summary>
        /// Virtual aync funciton that the user can override to provide
        /// any custom functionality required.
        /// </summary>
        /// <param name="gesture"><see cref="GestureResult"/></param>
        /// <param name="annoyingbaseobjectthing"></param>
        /// <returns></returns>
        protected virtual async Task Execute(GestureResult gesture, object annoyingbaseobjectthing)
        {
            await asyncExecute(gesture, annoyingbaseobjectthing as T);
        }
    }


}
