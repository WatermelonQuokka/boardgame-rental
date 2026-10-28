SELECT members.name AS member_name, boardgames.name AS boardgame_name, rentals.rental_start, rentals.rental_end
FROM rentals
JOIN boardgames ON rentals.game_id = boardgames.id
JOIN members ON rentals.member_id = members.id
ORDER BY rentals.rental_end