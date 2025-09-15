package be.brightest.ScreenPlay.Abilities;

/**
 * Represents an ability that an actor can have.
 */
public interface IAbility extends AutoCloseable {

    /**
     * Gets the name of the ability.
     * @return the name of the ability
     */
    String getName();
}
