
const updateButton = document.querySelector("#updateButton")
const updateDetails = document.querySelector("#updateDetails")
const email = document.querySelector("#email")
const psswrd = document.querySelector("#password")
const firstN = document.querySelector("#firstName")
const lastN = document.querySelector("#lastName")

document.addEventListener('DOMContentLoaded', async function () {
    
    
    const dataPost = await JSON.parse(localStorage.getItem('dataPost'))
    const firstName = dataPost['firstName']
    
    const enterMessage = document.getElementById('enterMessage')

    enterMessage.textContent = `שלום ${firstName}, מייד נצלול פנימה`

})
 function updatDetailsButton() {
    const inputBtns = document.querySelectorAll('input')
    inputBtns.forEach(input => {
        input.style.display = 'block'

    }
    );
     updateButton.style.display = 'block'

}
updateButton.addEventListener('click', async () => {
    try {
        if (psswrd.value.length < 4 || firstN.value.length < 2 || lastN.value.length < 2)
            throw Error("Unvalid details!")
    }
    catch (Error) {
        alert(Error + "  Please enter again your details!")
        return;
    }
    const dataPost = await JSON.parse(localStorage.getItem('dataPost'))
    const id = dataPost['userId']
    const dataPut =
    {
        email: email.value,
        password: psswrd.value,
        firstName: firstN.value,
        lastName: lastN.value,
        userId: id,
        body: "User updated details ",
        title: "Updating a user"
    }
    console.log(dataPut)
    const response = await fetch('/api/My/'+id,
        {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(dataPut)
        });


    try {
        if (!response.ok) {
            const errorDetails = await response.text();
            throw new Error('Error: ' + response.status + ' ' + errorDetails);
        }
    }
    catch (err) {
        alert(err.message );
        return;
    }


    const putData = await response.json()
    console.log('Put data:', putData)
    alert('Details updated successfully')
    localStorage.removeItem('dataPost', JSON.stringify(dataPost))
    localStorage.setItem('dataPost', JSON.stringify(putData))
}
)  

