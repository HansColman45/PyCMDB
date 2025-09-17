package be.brightest.ScreenPlay.Tasks;

import be.brightest.ScreenPlay.Actors.IActor;

/**
 * Represents a task that can be performed by an actor.
 */
public abstract class Tasks implements ITasks {
    /**
     * Perform the task as the given actor.
     * @param actor the actor who is performing the task
     */
    public abstract void performAs(IActor actor);
}
