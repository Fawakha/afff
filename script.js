function LoadItems(type) {
    fetch(`http://localhost:5284/inventory/${type}`)
        .then(response => response.json())
        .then(items => {
            RenderItems(items);
        })
        .catch(error => console.error('Помилка підключення до сервера:', error));
}

function AddNewItem() {
    const name = document.getElementById('item-name').value;
    const CategoryId = parseInt(document.getElementById('item-type').value);
    const wear = document.getElementById('item-wear').value;
    const price = document.getElementById('item-price').value;

    if (name && CategoryId && wear && price) {
        fetch('http://localhost:5284/inventory', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                name: name,
                CategoryId: CategoryId,
                wear: parseFloat(wear),
                price: parseFloat(price)
            })
        })
            .then(response => {
                if (response.ok) {
                    alert('Предмет додано успішно!');

                    document.getElementById('item-name').value = '';
                    document.getElementById('item-type').value = '1';
                    document.getElementById('item-wear').value = '';
                    document.getElementById('item-price').value = '';

                    LoadItems('All');
                    LoadStats();
                } else {
                    response.text().then(errorMessage => {
                        alert('Помилка від сервера: ' + errorMessage);
                    });
                }
            })
            .catch(error => {
                console.error('Помилка мережі:', error);
                alert('Не вдалося зв’язатися із сервером.');
            });
    } else {
        alert('Будь ласка, заповніть всі поля.');
    }
}

function DeleteItem(id) {
    fetch(`http://localhost:5284/inventory/${id}`, {
        method: 'DELETE'
    })
        .then(response => {
            if (response.ok) {
                alert('Предмет видалено успішно!');
                LoadItems('All');
                LoadStats();
            } else {
                alert('Не вдалося видалити предмет. Перевірте сервер.');
            }
        });
}

function UpdatePrice(id, name, categoryId, wear) {
    const newPrice = prompt('Введіть нову ціну:');
    if (!newPrice) return;
    fetch(`http://localhost:5284/inventory/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
            name: name,
            categoryId: categoryId,
            wear: wear,
            price: parseFloat(newPrice)
        })
    })
        .then(response => {
            if (response.ok) {
                alert('Ціна оновлена успішно!');
                LoadItems('All');
                LoadStats();
            } else {
                alert('Не вдалося оновити ціну. Перевірте сервер.');
            }
        });
}

function ApplyFilters() {
    const sortBy = document.getElementById('sortSelect').value;
    fetch(`http://localhost:5284/inventory/filter?sortBy=${sortBy}`)
        .then(response => response.json())
        .then(items => {
            RenderItems(items);
        })
        .catch(error => {
            console.error('Помилка підключення до сервера:', error);
        });
}

function LoadStats() {
    fetch('http://localhost:5284/inventory/stats')
        .then(response => response.json())
        .then(data => {
            console.log("Дані з сервера:", data);
            document.getElementById('stats-container').innerText =
                `Загальна кількість предметів: ${data.totalitems} | Загальна сума: ${data.totalsum}`;
        })
        .catch(error => {
            console.error('Помилка підключення до сервера:', error);
        });
}
LoadStats();

function RenderItems(items) {
    const container = document.getElementById('inventory-container');
    const categoryNames = { 1: 'Зброя', 2: 'Ножі', 3: 'Рукавиці' };
    container.innerHTML = '';
    items.forEach(item => {
        const card = document.createElement('div');
        card.className = 'card';
        card.innerHTML = `
            <h3>${item.name}</h3>
            <button onclick="DeleteItem(${item.id})">Видалити</button>
            <div class="item-name">${item.name}</div>
            <div class="item-type">${categoryNames[item.categoryId] || 'інше'}</div>
            <div class="wear">Wear: ${item.wear}</div>
            <div class="price">$${item.price}</div>
            <button onclick="UpdatePrice(${item.id}, '${item.name}', ${item.categoryId}, ${item.wear})">Оновити ціну</button>
        `;
        container.appendChild(card);
    });
}

function LoadCategories() {
    fetch('http://localhost:5284/categories')
        .then(response => response.json())
        .then(Categories => {
            const select = document.getElementById('item-type');
            select.innerHTML = '';
            Categories.forEach(Categories => {
                const option = document.createElement('option');
                option.value = Categories.id;
                option.textContent = Categories.name;
                select.appendChild(option);
            });
        })
        .catch(error => console.error('Помилка завантаження категорій:', error));
}
LoadCategories();
