import React from "react";
import { Text, StyleSheet } from "react-native";
import Container from "../../common/Container";
import Colors from "../../utils/colors";

const MontlyReport = props => {
  return (
    <Container style={styles.container}>
      <Text>Montly Report</Text>
    </Container>
  );
};
const styles = StyleSheet.create({
  container: {
    backgroundColor: "yellow",
    alignItems: "center",
    justifyContent: "center",
    padding: 50
  }
});

export default MontlyReport;
