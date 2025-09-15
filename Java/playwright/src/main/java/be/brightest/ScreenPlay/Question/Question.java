package be.brightest.ScreenPlay.Question;

import be.brightest.ScreenPlay.Actors.IActor;

/**
 * Represents a question that can be asked by an actor.
 * @param <T> the type of the answer to the question
 */
public abstract class Question<T> implements IQuestion<T>{
    private T value;

    /**
     * Perform the question as the given actor.
     * @param actor the actor who is asking the question
     * @return the answer to the question
     */
    public abstract T performAs(IActor actor);

    /**
     * Set the value of the question.
     * @param value the value to set
     */
    public void set(T value){
        this.value = value;
    }

    /**
     * Get the value of the question.
     * @return the value of the question
     */
    public T get(){
        return value;
    }
}
