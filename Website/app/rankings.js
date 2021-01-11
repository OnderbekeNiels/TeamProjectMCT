const showTable = function (data) {
  console.log("table data received");
  console.table(data);
};

const getRounds = async function () {
  let endpoint = `https://temptrackingfunction.azurewebsites.net/api/gebruikers/ronde/547F309B-8596-4DBE-9439-333A7C9E79DE?code=WJ/wMMoTjMGaF6AdEBO9gyjfMaODsitooxxbpAavwzUhEj4WcgrLqw==`;
  try {
    const response = await fetch(endpoint);
    const data = await response.json();
    showTable(data);
  } catch (error) {
    console.error("An error occured, we handled it.", error);
  }
};

document.addEventListener("DOMContentLoaded", function () {
  console.log("DOM loaded :)");

  getRounds();
});
