import React from "react";
import { Text, StyleSheet } from "react-native";
import Container from "../../common/Container";
import Colors from "../../utils/colors";

const SiteReport = props => {
  return (
    <Container style={styles.container}>
      <Text>Site Report</Text>
    </Container>
  );
};
const styles = StyleSheet.create({
  container: {
    backgroundColor: Colors.lightGreen,
    alignItems: "center",
    justifyContent: "center",
    padding: 50
  }
});

export default SiteReport;
