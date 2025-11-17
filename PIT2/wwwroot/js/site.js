// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let data = sessionStorage.getItem("key");
if (!data) {
    sessionStorage.setItem("key", crypto.randomUUID());
} 

const modal = document.getElementById('productModal');
const productImage = document.getElementById('productImage');
const productName = document.getElementById('productName');
const productPrice = document.getElementById('productPrice');
const productDescription = document.getElementById('productDescription');
const productQtd = document.getElementById('productQtd');

let produto = {};
let carrinho = [];

function openModal(button) {
    produto = {};
    productName.textContent = produto.Name = button.getAttribute('data-nome');
    var preco = button.getAttribute('data-preco');
    productPrice.textContent = `R$ ${preco}`;
    produto.Price = preco;
    productDescription.textContent = button.getAttribute('data-descricao');
    productImage.src = produto.Photo = button.getAttribute('data-imagem');
    productQtd.value = 0;

    produto.Id = button.getAttribute('data-id');

    modal.style.display = 'block';
}

function addToCart() {
    debugger;
    var qtd = productQtd.value;
    if (!qtd) {
        return alert('Adicione a quantidade de cupcakes desejada ao carrinho.')
    }

    const index = carrinho.findIndex(item => item.Id === produto.Id);
    if (index != -1 && qtd) {
        carrinho[index].Quantity = qtd;
        atualizaCarrinhoStorage(carrinho);
        return alert('Produto Adicionado com sucesso.', closeModal())
    }

    produto.Quantity = qtd;

    carrinho.push(produto);
    atualizaCarrinhoStorage(carrinho);

    return alert('Produto Adicionado com sucesso.', closeModal())
}

function closeModal() {
    modal.style.display = 'none';
}

function atualizaCarrinhoStorage(carrinho) {
    sessionStorage.setItem("carrinho", JSON.stringify(carrinho));
}

window.onclick = function (event) {
    if (event.target === modal) {
        closeModal();
    }
}