package be.hans.cmdb.Questions;

import be.brightest.ScreenPlay.Actors.IActor;
import be.brightest.ScreenPlay.Question.Question;
import org.hibernate.HibernateException;
import org.hibernate.Session;
import org.hibernate.SessionFactory;
import org.hibernate.cfg.Configuration;

public class OpenDbConnection extends Question<Session> {

    @Override
    public Session performAs(IActor actor) {
        try {
            SessionFactory sessionFactory = new Configuration()
                .configure()
                .buildSessionFactory();
            return sessionFactory.openSession();
        } catch (HibernateException e) {
            throw new RuntimeException(e);
        }
    }
}
