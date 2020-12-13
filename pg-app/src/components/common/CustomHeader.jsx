import React from "react";
import { Ionicons } from "@expo/vector-icons";
import { View, StyleSheet } from "react-native";

const CustomHeader = props => {
  return (
    <View style={styles.container}>
      <Ionicons
        name="md-menu"
        size={32}
        color="#000"
        onPress={() => props.navigation.openDrawer()}
      />
    </View>
  );
};

const styles = StyleSheet.create({
  container: {
    borderBottomWidth: 2,
    height: 70,
    paddingTop: 30
  }
});

export default CustomHeader;
