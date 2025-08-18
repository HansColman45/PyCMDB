package be.cmdb.dao;

import be.cmdb.model.Category;
import be.cmdb.model.Log;
import org.hibernate.Session;
import org.hibernate.query.Query;

import java.util.List;

public class CategoryDAO implements BaseDAO<Category, Integer> {
    @Override
    public Category save(Session session, Category entity) {
        session.persist(entity);
        return entity;
    }

    @Override
    public Category update(Session session, Category entity) {
        session.merge(entity);
        return entity;
    }

    @Override
    public Category findById(Session session, Integer id) {
        return session.find(Category.class, id);
    }

    @Override
    public List<Category> findAll(Session session) {
        Query<Category> query = session.createQuery("FROM Category", Category.class);
        return query.getResultList();
    }

    @Override
    public void delete(Session session, Category entity) {
        session.remove(entity);
    }

    @Override
    public void deleteById(Session session, Integer id) {
        Category category = findById(session, id);
        if (category != null) {
            delete(session, category);
        }
    }

    @Override
    public long count(Session session) {
        Query<Long> query = session.createQuery("SELECT COUNT(c) FROM Category c", Long.class);
        return query.uniqueResult();
    }

    /**
     * Finds categories by their c.
     * @param session the Hibernate session to use
     * @param category the name of the category to search for
     * @return a list of Category entities matching the given name
     */
    public List<Category> findByCategory(Session session, String category) {
        Query<Category> query = session.createQuery("FROM Category WHERE name = :categoryName", Category.class);
        query.setParameter("categoryName", category);
        return query.getResultList();
    }

}
