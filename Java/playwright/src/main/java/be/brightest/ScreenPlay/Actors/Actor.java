package be.brightest.ScreenPlay.Actors;

import be.brightest.ScreenPlay.Abilities.IAbility;
import be.brightest.ScreenPlay.Question.IQuestion;
import be.brightest.ScreenPlay.Question.Question;
import be.brightest.ScreenPlay.Tasks.ITasks;
import be.brightest.ScreenPlay.Tasks.Tasks;

import java.util.ArrayList;
import java.util.List;

/**
 * Represents an actor in the ScreenPlay pattern.
 * An actor can have multiple abilities and can ask questions.
 */
public class Actor implements IActor{
    private final String name;
    private final List<IAbility> abilities;

    /**
     * Constructs an Actor with the given name.
     * @param name the name of the actor
     * @throws IllegalArgumentException if the name is null or empty
     */
    public Actor(String name) {
        if (name == null || name.isEmpty()) {
            throw new IllegalArgumentException("Actor name cannot be null or empty");
        }
        this.name = name;
        abilities = new ArrayList<IAbility>();
    }

    @Override
    public String getName() {
        return name;
    }

    @Override
    public List<IAbility> getAbilities() {
        return abilities;
    }

    @Override
    public void addAbility(IAbility ability) {
        abilities.add(ability);
    }

    @Override
    public boolean isAbleToUseOrDo(IAbility ability){
        return abilities.contains(ability);
    }

    /**
     * Get an ability of the actor by its class.
     * @param abilityClass the class of the ability to get
     * @return the ability of the actor
     * @throws IllegalStateException if the actor does not have the ability
     */
    public IAbility getAbility(Class<? extends IAbility> abilityClass){
        for (IAbility ability : abilities) {
            if (ability.getClass().equals(abilityClass)) {
                return ability;
            }
        }
        throw new IllegalStateException("Actor " + name + " does not have the ability " + abilityClass.getSimpleName());
    }

    @Override
    public <T> T asksFor(IQuestion<T> question) {
        if (question instanceof Question) {
            T result = ((Question<T>) question).performAs(this);
            if (result instanceof IAbility){
                addAbility((IAbility) result);
            }
            return result;
        }
        throw new IllegalArgumentException("Unknown question type: " + question.getClass().getName());
    }

    @Override
    public void asksFor(ITasks task){
        if (task instanceof Tasks) {
            ((Tasks) task).performAs(this);
        } else {
            throw new IllegalArgumentException("Unknown task type: " + task.getClass().getName());
        }
    }
}
