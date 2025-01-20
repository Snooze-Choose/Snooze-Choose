<script lang="ts">
interface WeatherForecast {
  id: string
  name: string
}

type Forecasts = WeatherForecast[]

export default {
  name: 'TheWelcome',
  data() {
    return {
      forecasts: [],
      loading: true,
      error: null
    }
  },
  mounted() {
    fetch('api/products')
      .then((response) => response.json())
      .then((data) => {
        this.forecasts = data
      })
      .catch((error) => {
        this.error = error
      })
      .finally(() => (this.loading = false))
  }
}
</script>

<template>
  <table>
    <thead>
      <tr>
        <th>id</th>
        <th>Name</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="forecast in forecasts as Forecasts">
        <td>{{ forecast.id }}</td>
        <td>{{ forecast.name }}</td>
      </tr>
    </tbody>
  </table>
</template>

<style>
table {
  border: none;
  border-collapse: collapse;
}

th {
  font-size: x-large;
  font-weight: bold;
  border-bottom: solid 0.2rem hsla(160, 100%, 37%, 1);
}

th,
td {
  padding: 1rem;
}

td {
  text-align: center;
  font-size: large;
}

tr:nth-child(even) {
  background-color: var(--vt-c-black-soft);
}
</style>
