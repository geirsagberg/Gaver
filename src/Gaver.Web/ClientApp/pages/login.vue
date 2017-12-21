<template>
  <div class="container">
  <!-- {this.props.isLoggingIn ? (
    <Loading />
  ) : ( -->
    <div class="well col-sm-6 col-centered">
      <h1 class="headline">Gaver</h1>
      <button class="btn btn-primary" v-bind:click="logIn">
	Logg inn
      </button>
    </div>
  <!-- )} -->
  </div>
</template>

<<script lang="ts">
export default {
  logIn () {
    console.log('Logging in')
  }
}
</script>
