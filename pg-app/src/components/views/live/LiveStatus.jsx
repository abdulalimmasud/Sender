import React from "react";
import { Text, StyleSheet } from "react-native";
import Container from "../../common/Container";
import Colors from "../../utils/colors";

const LiveStatus = props => {
  return (
    <Container style={styles.container}>
      <Text>Hello, Welcome to LiveStatus</Text>
    </Container>
  );
};
const styles = StyleSheet.create({
  container: {
    backgroundColor: Colors.lightBlue,
    alignItems: "center",
    justifyContent: "center"
  }
});

export default LiveStatus;
