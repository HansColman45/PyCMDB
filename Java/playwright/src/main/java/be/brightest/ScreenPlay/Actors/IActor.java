package be.brightest.ScreenPlay.Actors;

import be.brightest.ScreenPlay.Abilities.IAbility;
import be.brightest.ScreenPlay.Question.IQuestion;

import java.util.List;

public interface IActor {
    /**
     * Get the name of the actor.
     * @return the name of the actor
     */
    String getName();

    /**
     * Set the name of the actor.
     * @return list of abilities the actor has
     */
    List<IAbility> getAbilities();

    /**
     * Add an ability to the actor.
     * @param ability the ability to add
     */
    void addAbility(IAbility ability);

    /**
     * Check if the actor is able to use or do a certain ability.
     * @param ability the ability to check
     * @return true if the actor is able to use or do the ability, false otherwise
     */
    boolean isAbleToUseOrDo(IAbility ability);

    /**
     * Get an ability of the actor by its class.
     * @param abilityClass the class of the ability to get
     * @return the ability of the actor
     */
    IAbility getAbility(Class<? extends IAbility> abilityClass);

    /**
     * Ask a question to the actor.
     * @param question the question to ask
     * @param <T> the type of the answer
     * @return the answer to the question
     */
    <T> T asksFor(IQuestion<T> question);
}
