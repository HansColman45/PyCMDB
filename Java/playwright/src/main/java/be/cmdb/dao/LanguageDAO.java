package be.cmdb.dao;

import be.cmdb.model.Language;
import java.util.List;
import org.hibernate.Session;
import org.hibernate.query.Query;

/**
 * Data Access Object for Language entity operations.
 */
public class LanguageDAO implements BaseDAO<Language, String> {

    @Override
    public Language save(Session session, Language entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public Language update(Session session, Language entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public Language findById(Session session, String id) {
        return session.find(Language.class, id);
    }

    @Override
    public List<Language> findAll(Session session) {
        return session.createQuery("FROM Language", Language.class).getResultList();
    }

    @Override
    public void delete(Session session, Language entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, String id) {
        Language language = findById(session, id);
        if (language != null) {
            delete(session, language);
        } else {
            // Optionally handle the case where the mobile is not found
            System.out.println("Mobile with ID " + id + " not found.");
        }
    }

    @Override
    public long count(Session session) {
        Query<Long> query = session.createQuery("SELECT COUNT(*) FROM Language l", Long.class);
        return query.getSingleResult();
    }

    /**
     * Finds a Language by its code.
     * @param session the Hibernate session
     * @param code the language code to search for
     * @return the Language entity matching the code, or null if not found
     */
    public Language findByCode(Session session, String code) {
        String hql = "FROM Language WHERE code = :code";
        Query<Language> query = session.createQuery(hql, Language.class);
        query.setParameter("code", code);
        return query.getSingleResult();
    }
}
