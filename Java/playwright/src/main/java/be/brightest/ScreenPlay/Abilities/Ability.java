package be.brightest.ScreenPlay.Abilities;

public class Ability implements IAbility{
    @Override
    public String getName() {
        return Ability.class.getName();
    }

    @Override
    public void close() throws Exception {

    }
}
